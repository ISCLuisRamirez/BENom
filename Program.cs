using BENom.Data;
using System.Text;
using BENom.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos MySQL
builder.Services.AddDbContext<BENomDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddAuthorization();

// Configuración de JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Migrar la base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BENomDbContext>();
    db.Database.EnsureCreated();
}

// Endpoint para registrar usuarios
app.MapPost("/register", async (User user, BENomDbContext db) =>
{
    if (await db.Users.AnyAsync(u => u.employee_number == user.employee_number))
        return Results.BadRequest("El usuario ya existe.");

    user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok("Usuario registrado con éxito.");
});

// Endpoint para autenticar usuarios
app.MapPost("/login", async (User user, BENomDbContext db) =>
{
    var existingUser = await db.Users.FirstOrDefaultAsync(u => u.employee_number == user.employee_number);
    if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.password, existingUser.password))
        return Results.Unauthorized();

    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim("id", existingUser.id.ToString()),
            new System.Security.Claims.Claim("role", existingUser.id_cat_role.ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = jwtIssuer,
        Audience = jwtIssuer,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    return Results.Ok(new { Token = tokenString });
});

app.Run();
