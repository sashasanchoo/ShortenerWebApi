using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortener.Data;
using Shortener.Model;
using Shortener.Services;
using Shortener.ViewModel;

namespace Shortener.Controllers
{
    [EnableCors("Client", PolicyName = "Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenerURLController : ControllerBase
    {
        private readonly ShortenerContext _context;
        private readonly SecureRandomStringProvider _secureRandomStringProvider;
        public ShortenerURLController(ShortenerContext context, SecureRandomStringProvider secureRandomStringProvider)
        {
            _context = context;
            _secureRandomStringProvider = secureRandomStringProvider;
        }

        // GET: api/ShortenerURL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortenerURL>>> GetShortenerURLsList()
        {
            return await _context.ShortenerURLs.ToListAsync();
        }

        // POST: api/ShortenerURL
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<ShortenerHolder>> CreateShortenerURL(URLHolder urlHolder)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            if(await IsURLShortenerExists(urlHolder.URL)) 
            { 
                return BadRequest("Shortener for entered URL is already exist");
            }
            var shortenerURL = new ShortenerURL()
            {
                CreatedBy = User.Identity?.Name,
                CreatedDate = DateTime.Now,
                URL = urlHolder.URL,
                Shortener = await _secureRandomStringProvider.CreateSecureRandomString()
            };
            await _context.ShortenerURLs.AddAsync(shortenerURL);
            await _context.SaveChangesAsync();
            return new ShortenerHolder { Shortener = shortenerURL.Shortener};
        }
        // POST: api/ShortenerURL/UseShortenerUrl
        [HttpPost("UseShortenerUrl")]
        public async Task<ActionResult<URLHolder>> RedirectBasedOnShortener(ShortenerHolder shortenerHolder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var existingShortenerURL = await _context.ShortenerURLs.FirstOrDefaultAsync(s => s.Shortener == shortenerHolder.Shortener);
            if(existingShortenerURL == null)
            {
                return BadRequest("URL for given shortener has been not found.");
            }
            return new URLHolder { URL = existingShortenerURL.URL };
        }
        // DELETE: api/ShortenerURL/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShortener(int id)
        {
            var shortener = await _context.ShortenerURLs.FindAsync(id);
            if(shortener == null)
            {
                return BadRequest("The URL with given identifier has been not found.");
            }
            _context.ShortenerURLs.Remove(shortener);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // GET: api/ShortenerURL/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShortenerURL>> GetConcreteShortener(int id)
        {
            var shortener = await _context.ShortenerURLs.FindAsync(id);
            if (shortener == null)
            {
                return BadRequest("The URL with given identifier has been not found.");
            }
            return shortener;
        }
        [NonAction]
        private async Task<bool> IsURLShortenerExists(string URL)
        {
            return await _context.ShortenerURLs.Where(s => s.URL == URL).AnyAsync();
        }
    }
}