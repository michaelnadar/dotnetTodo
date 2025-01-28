using Backend.DTOs;
using Backend.Models;
using Backend.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

        private readonly IAuthService authService;
        
        public AuthController(IAuthService auth_service)
        {   
                authService = auth_service;
               
        }


       [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserDto request)
        {
            var result = await authService.RegisterAsync(request);
            if (result is null)
                return BadRequest("Username already exists.");

            return Ok(result);
        }

         [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            //return CreateToken(request)
            return Ok(result);
        }

        [Authorize]
         [HttpGet]
        public ActionResult Auth( )
        {
            
            return Ok("you are authorized!");
        }

       

}