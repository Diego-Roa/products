using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskManagement.DataAccess.Entities;
using TaskManagement.Services.DTOs;
using TaskManagement.Services.Services;

namespace TaskManagement.Controllers
{
    /// <summary>
    /// Controlador REST para inicio de sesión de usuarios
    /// </summary>
    /// 
    /// <p> Autor: Sebastian Roa </p>
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class LoginController
    {

        private readonly RoleManager<AplicationRoleEntity> _roleManager;
        private readonly UserManager<AplicationUserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly LoginService loginService;

        public LoginController(RoleManager<AplicationRoleEntity> roleManager, UserManager<AplicationUserEntity> userManager, IConfiguration configuration, LoginService loginService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            this.loginService = loginService;
        }

        /// <summary>
        /// Endpoint para inicio de sesion del usuario
        /// </summary>
        /// <param name="loginDto">DTO con el username y password</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseDTO<TokenDTO>> Login([FromBody] LoginDTO loginDto)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            return await loginService.Login(loginDto, jwtSettings);

        }
    }
}
