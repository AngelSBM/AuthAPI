using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Auth.ClientLayer.Helpers.Exceptions;
using Auth.ClientLayer.Helpers.Utilities;

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
                var  createdUser = _authService.RegisterUser(newUser);

                return ApiResponse.OK(createdUser);

            }
            catch (BadRequestException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.BadRequest(resp); 
            }   
            catch (Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, "Error interno del servidor");
            }                        
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginDTO user)
        {         
            try
            {
                UserCrendentialsDTO tokens = _authService.Login(user);

                var cookieOptions = new CookieOptions { HttpOnly = true };
                Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);


                return ApiResponse.OK(new
                {
                    Message = "You are logged!",
                    AccessToken = tokens.AccessToken
                });
            }
            catch (BadRequestException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.BadRequest(resp);
            }
            catch (NotFoundException e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return ApiResponse.NotFound(resp);
            }
            catch(Exception e)
            {
                var resp = ApiResponse.CreateErrorObject(e.Message);
                return StatusCode(500, "Error interno del servidor");
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
                return new NotFoundResult();
            }
            
        }

    }
}
