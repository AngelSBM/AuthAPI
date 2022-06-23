using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Auth.ClientLayer.Controllers
{

    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;        
        public AuthController(IAuthService _authService) 
        {
            this._authService = _authService;
        }

        
        [HttpPost("Register")]
        public IActionResult Register(UserRegisterDTO newUser)
        {
            try
            {
                _authService.RegisterUser(newUser);
                return new OkObjectResult(true);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }                        
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginDTO user)
        {

            try
            {
                UserCrendentialsDTO tokens = _authService.Login(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                };

                Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);

                var response = new OkObjectResult(new
                {
                    Message = "You are logged!",
                    AccessToken = tokens.AccessToken
                });


                return response;


                //response.
            }
            catch (Exception e)
            {

                return new BadRequestObjectResult(e.Message);
            }            
        }

        [HttpPost("refreshToken")]        

        public IActionResult RefreshToken()
        {

            try
            {
                UserCrendentialsDTO newTokens = _authService.RefreshSession();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                };

                Response.Cookies.Append("refreshToken", newTokens.RefreshToken, cookieOptions);

                var response = new OkObjectResult(new
                {
                    Message = "Token refreshed",
                    AccessToken = newTokens.AccessToken
                });

                return Ok(response);
            }
            catch (Exception e)
            {

                throw;
            }

            
        }

    }
}
