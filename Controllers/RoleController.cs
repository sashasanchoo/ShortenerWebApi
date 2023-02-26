using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortener.Model;

namespace Shortener.Controllers
{
    [EnableCors("Client", PolicyName = "Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, IRoleStore<IdentityRole> roleStore, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
            _userManager = userManager;
        }
        // GET: api/Role
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        // POST: api/Role
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateRole(InputRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            }
            return Ok();
        }
        // GET: api/Role/IsAdmin
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}")]
        [HttpGet("IsAdmin")]
        public async Task<ActionResult> IsAdmin()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(await _userManager.IsInRoleAsync(user, "Admin"));
        }
    }
}
