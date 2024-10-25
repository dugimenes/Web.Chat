using Blog.Data.Models;
using Blog.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Blog.Web.Configuration
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAutorService _autorService;

        public CustomSignInManager(UserManager<ApplicationUser> userManager,
                                    IHttpContextAccessor contextAccessor,
                                    IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                                    IOptions<IdentityOptions> optionsAccessor,
                                    ILogger<SignInManager<ApplicationUser>> logger,
                                    IAuthenticationSchemeProvider schemes,
                                    IUserConfirmation<ApplicationUser> confirmation,
                                    IAutorService autorService)
                                    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
                                    {
                                     _userManager = userManager;
                                     _autorService = autorService;
                                    }

        public async Task<IdentityResult> CustomCreateUserAsync(ApplicationUser user, string password)
        {
            var result = await UserManager.CreateAsync(user, password); // Use UserManager do SignInManager
            if (result.Succeeded)
            {
                await _autorService.CreateAutorAsync(user);
            }
            return result;
        }
    }
}