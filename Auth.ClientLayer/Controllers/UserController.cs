using Auth.LogicLayer.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userService.getUsers();

            try
            {
                return new OkObjectResult(users);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
