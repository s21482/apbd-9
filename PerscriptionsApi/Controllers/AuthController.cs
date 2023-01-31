using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PerscriptionsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PerscriptionsApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly MainDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MainDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(LoginRequest request)
        {
            Console.WriteLine(request);
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string hashed = HashPassword(request.Password, salt);

            string saltBase64 = Convert.ToBase64String(salt);

            var newUser = new User
            {
                Login = request.Login,
                Password = hashed,
                Salt = saltBase64,
                Role = "user",
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpirationDate = DateTime.Now.AddDays(7),
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginRequest.Login);
            if (user == null)
            {
                return Unauthorized();
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            string hash = HashPassword(loginRequest.Password, salt);

            if (user.Password != hash)
            {

                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"])); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpirationDate = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashed;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<string>> RefreshToken(String token)
        {
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.RefreshToken == token);
                if (user == null)
                {
                    return BadRequest();
                }
                if (user.RefreshTokenExpirationDate < DateTime.Now)
                {
                    token = user.RefreshToken;
                    user.RefreshToken = Guid.NewGuid().ToString();
                    user.RefreshTokenExpirationDate = DateTime.Now.AddDays(7);

                    await _context.SaveChangesAsync();
                }
                return Ok(token);
            }
        }
    }
}
