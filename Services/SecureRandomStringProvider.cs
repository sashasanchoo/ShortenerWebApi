using Microsoft.EntityFrameworkCore;
using Shortener.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Shortener.Services
{
    public class SecureRandomStringProvider
    {
        private const int COUNT = 8;
        private readonly ShortenerContext _context;
        public SecureRandomStringProvider(ShortenerContext context)
        {
            _context = context;
        }
        public async Task<string> CreateSecureRandomString()
        {
            var randomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(COUNT));
            while(await _context.ShortenerURLs.Where(s => s.Shortener == randomString).AnyAsync())
            {
                randomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(COUNT));
            }
            return randomString;
        }    
    }
}
