using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.UseCases.Users.LoginUser;
using CommerceCleanArchitectureNET.Application.UseCases.Users.RegisterUser;
using CommerceCleanArchitectureNET.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceCleanArchitectureNET.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUser;
        private readonly ILoginUserUseCase _loginUser;

        public AuthController(IRegisterUserUseCase registerUser, ILoginUserUseCase loginUser)
        {
            _registerUser = registerUser;
            _loginUser = loginUser;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken ct)
        {
            var result = await _registerUser.ExecuteAsync(dto, ct);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Error!));

            return CreatedAtAction(nameof(Register), result.Value);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken ct)
        {
            var result = await _loginUser.ExecuteAsync(dto, ct);

            if (!result.IsSuccess)
                return Unauthorized(new ErrorResponse(result.Error!));

            return Ok(result.Value);
        }
    }
}
