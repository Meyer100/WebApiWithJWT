using BusinessLogic;
using CLModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiWithJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {

        // Injecting UserRepository
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Allow anonymous allows this method to be run without authorization
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            string token = await _userRepository.Login(userDTO);
            if(token == null)
            {
                return BadRequest("Token blev ikke oprettet");
            }
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> ReadClaimsValues()
        {
            string usernameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
            string UID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(usernameClaim == null || UID == null)
            {
                return NotFound("Hov der skete en fejl, værdierne blev ikke fundet");
            }
            return Ok($"Brugernavn: {usernameClaim}, UID: {UID}");
        }
    }
}
