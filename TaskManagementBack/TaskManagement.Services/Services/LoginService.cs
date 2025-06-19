using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DataAccess.Entities;
using TaskManagement.Services.DTOs;

namespace TaskManagement.Services.Services
{
    /// <summary>
    /// Servicio para gestionar operaciones relacionadas con el login.
    /// </summary>
    public class LoginService
    {

        private readonly RoleManager<AplicationRoleEntity> _roleManager;
        private readonly UserManager<AplicationUserEntity> _userManager;


        public LoginService(RoleManager<AplicationRoleEntity> roleManager, UserManager<AplicationUserEntity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Servicio para ingresar al aplicativo por su usuario.
        /// </summary>
        public async Task<ResponseDTO<TokenDTO>> Login(LoginDTO loginDto, IConfigurationSection config)
        {
            // Buscar el usuario por nombre de usuario
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                return new ResponseDTO<TokenDTO>(false, "Nombre de usuario o contraseña incorrectos.", null);

            // Verificar la contraseña
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return new ResponseDTO<TokenDTO>(false, "Nombre de usuario o contraseña incorrectos.", null);

            // Obtener roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            // Crear claims
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Agregar roles como claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Leer configuraciones de JWT
            
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Key"]));

            // Crear el token JWT
            var token = new JwtSecurityToken(
                issuer: config["Issuer"],
                audience: config["Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["ExpiresInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            TokenDTO tokenDto = new TokenDTO();
            tokenDto.Token = tokenString;
            tokenDto.Expiration = token.ValidTo;

            // Retornar el token y la expiración
            return new ResponseDTO<TokenDTO>(true, "success", tokenDto);

        }

        /// <summary>
        /// Servicio para actualizar los datos del usuario.
        /// </summary>
        public async Task<ResponseDTO> updateUser(UserCreateDTO userCreateDTO)
        {
            var user = await _userManager.FindByIdAsync(userCreateDTO.Id);
            user.FullName = userCreateDTO.FullName;
            user.Email = userCreateDTO.Username;
            user.UserName = userCreateDTO.Username;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDTO(false, "Error al intentar guardar el correo del usuario");
            }
            var roles = await _userManager.GetRolesAsync(user);
            result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return new ResponseDTO(false, "Error al intentar guardar el rol del usuario");
            }
            user = await _userManager.FindByIdAsync(userCreateDTO.Id);
            var resultRole = await _userManager.AddToRoleAsync(user, userCreateDTO.Role);
            if (!resultRole.Succeeded)
            {
                return new ResponseDTO(false, "Error al intentar guardar el rol del usuario");
            }
            return new ResponseDTO(true, "Usuario editado correctamente");
        }

    }
}
