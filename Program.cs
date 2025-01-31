using BENom.Data;
using System.Text;
using BENom.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();

        // Agregar parametros para producción //
        /* policy.WithOrigins("https://example.com")
            .AllowAnyMethod()
            .AllowAnyHeader(); */
    });
});

// Configuración de la base de datos MySQL
builder.Services.AddDbContext<BENomDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddAuthorization();
builder.Services.AddControllers();

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

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Migrar la base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BENomDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();

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
    var loginUser = await db.Users.FirstOrDefaultAsync(u => u.employee_number == user.employee_number);
    
    if (loginUser == null || !BCrypt.Net.BCrypt.Verify(user.password, loginUser.password))
        return Results.Unauthorized();

    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim("id", loginUser.id.ToString()),
            new System.Security.Claims.Claim("role", loginUser.id_role.ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = jwtIssuer,
        Audience = jwtIssuer,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    var RoleId = loginUser.id_role switch
    {
        1 => "admin",
        2 => "comite",
        3 => "capturista",
        _ => "Sin Rol"
    };
    
    return Results.Ok(new { 
        Id = loginUser.id,
        RoleId,
        EmployNumber = loginUser.employee_number,
        Email = loginUser.email,
        Token = tokenString 
    });
});

app.Run();
