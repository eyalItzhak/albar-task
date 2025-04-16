using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DAL.Context;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ping-db")]
        public async Task<IActionResult> PingDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                    return Ok("✅ DB is connected!");
                else
                    return StatusCode(500, "❌ DB connection failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Exception: {ex.Message}");
            }
        }
    }
}
