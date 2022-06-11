using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.ClientLayer.Controllers
{

    [ApiController]
    [Route("Auth")]
    public class AuthController
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
                _userService.Login(user);

                return new OkObjectResult("You are logged!");
            }
            catch (Exception e)
            {

                return new BadRequestObjectResult(e.Message);
            }            
        }

    }
}
