using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;


namespace FirstAPI.Controllers
{


    [ApiController]
    [Route("/signin-google")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync(
                GoogleDefaults.AuthenticationScheme, "access_token");
            _logger.LogInformation("Access Token: {AccessToken}", accessToken);
         }
    }
}