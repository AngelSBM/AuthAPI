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
        private readonly IUserService _userService;        
        public AuthController(IUserService userService) 
        {
            this._userService = userService;
        }


        //TODO: create funcionality which allows users registration
        [HttpPost("Register")]
        public IActionResult Register(UserRegisterDTO newUser)
        {
            try
            {
                _userService.RegisterUser(newUser);
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
                UserCrendentialsDTO tokens = _userService.Login(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                };

                Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);

                var response = new OkObjectResult(new
                {
                    Message = "You are logged!",
                    Token = tokens.AccessToken
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
        [Authorize]
        public IActionResult RefreshToken()
        {
            var userId = User.FindFirstValue("UID");
            return Ok(userId);
        }

    }
}
