using Blog.Data.Models;
using Blog.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAutorService _autorService;

        public AuthController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings, IAutorService autorService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _autorService = autorService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new ApplicationUser
            {

                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _autorService.CreateAutorAsync(user);
                await _signInManager.SignInAsync(user, false);
                return Ok(await GerarJwt(registerUser.Email));
            }

            return Problem("Falha ao registrar o usuário");
        }

        [HttpPost("logar")]
        public async Task<IActionResult> Logar(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }

            return Problem("Usuário ou senha incorretos");
        }

        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}