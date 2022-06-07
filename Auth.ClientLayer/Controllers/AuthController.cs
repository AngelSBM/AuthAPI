using Auth.LogicLayer.Abstractions;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetUsers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        //TODO: create funcionality which allows users registration

    }
}
