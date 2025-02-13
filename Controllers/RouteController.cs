using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BENom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoutesController : ControllerBase
    {
        private readonly EndpointDataSource _endpointDataSource;

        public RoutesController(EndpointDataSource endpointDataSource)
        {
            _endpointDataSource = endpointDataSource;
        }

        [HttpGet("list")]
        public IActionResult GetRoutes()
        {
            var routes = _endpointDataSource.Endpoints
                .Select(endpoint => endpoint.DisplayName)
                .ToList();

            return Ok(routes);
        }
    }
}
