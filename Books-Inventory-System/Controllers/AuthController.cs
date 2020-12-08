using System.Threading.Tasks;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.User;
using Books_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Books_Inventory_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<int> response = await authRepository.Register(
                new User { Username = request.Username }, request.Password
            );

            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<string> response = await authRepository.Login(
                request.Username, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
