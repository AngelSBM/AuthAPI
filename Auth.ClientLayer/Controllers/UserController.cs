using Auth.ClientLayer.Helpers.Utilities;
using Auth.LogicLayer.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.ClientLayer.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }


        [HttpGet("GetUsers")]
        //[Authorize]
        public IActionResult GetAllUsers()
        {

            try
            {
                var users = _userService.GetUsersDetail();
                return ApiResponse.OK(users);
            }
            catch (Exception e)
            {


                throw;
            }

        }

    }
}
