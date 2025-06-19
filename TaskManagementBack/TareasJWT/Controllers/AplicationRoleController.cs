using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.DataAccess;
using TaskManagement.DataAccess.Entities;
using TaskManagement.Services.Services;
using TaskManagement.Services.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskManagement.Controllers
{
    /// <summary>
    /// Controlador REST para gestionar usuarios y roles
    /// Proporciona endpoints para obtener, crear, editar y eliminar usuariso y roles
    /// 
    /// <p> Autor: Sebastian Roa </p>
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<AplicationRoleEntity> _roleManager;
        private readonly UserManager<AplicationUserEntity> _userManager;
        private readonly PermissionService permissionService;
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly LoginService loginService;

        public ApplicationRoleController(RoleManager<AplicationRoleEntity> _roleManager,
        UserManager<AplicationUserEntity> _userManager, PermissionService permissionService, ApplicationDbContext context,
        UnitOfWork unitOfWork, IConfiguration configuration, LoginService loginService)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this.context = context;
            this.permissionService = permissionService;
            this.unitOfWork = unitOfWork;
            _configuration = configuration;
            this.loginService = loginService;
        }

        /// <summary>
        /// Endpoint para crear roles de usuario.
        /// </summary>
        /// <param name="role">Nombre del rol a crear</param>
        [HttpGet("createRole")]
        public async Task createRole(string role)
        {
            var applicationRole = new AplicationRoleEntity { Name = role };
            if (!await _roleManager.RoleExistsAsync(applicationRole.Name))
            {
                var result = await _roleManager.CreateAsync(applicationRole);
            }
        }

        /// <summary>
        /// Endpoint para obtener todos los roles
        /// </summary>
        /// <returns> Una lista con los roles de usuario.</returns>
        [HttpGet("getRoles")]
        public ResponseDTO<List<AplicationRoleEntity>> getRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return new ResponseDTO<List<AplicationRoleEntity>>(true, "success", roles);
        }

        /// <summary>
        /// Endpoint para adicionar rol
        /// </summary>
        /// <param name="roleName">Nombre del rol a adicionar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpGet("addRole")]
        public async Task<ResponseDTO> addRole(string roleName)
        {
            var result = await _roleManager.FindByNameAsync(roleName);

            if (result != null) return new ResponseDTO(false, "El role ya existe.");
            var role = new AplicationRoleEntity();
            role.Name = roleName;
            await _roleManager.CreateAsync(role);
            return new ResponseDTO(true, "Role agregado correctamente");
        }

        /// <summary>
        /// Endpoint para adicionar rol a usuario 
        /// </summary>
        /// <param name="userId">Id del usuario al que se le agregara el rol</param>
        /// <param name="role">Rol a asignar al usuario</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpGet("addRoleToUser")]
        public async Task<ResponseDTO> addRoleToUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.FirstOrDefault() != null)
            {
                await _userManager.RemoveFromRoleAsync(user, roles.First());
            }
            var result = await _userManager.AddToRoleAsync(user, role);

            return new ResponseDTO(true, "Role asignado correctamente");
        }

        /// <summary>
        /// Endpoint para obtener todos los permisos por usuario
        /// </summary>
        /// <returns> Una lista con los permisos del usuario</returns>
        [HttpGet("permissionsUser")]
        public async Task<ResponseDTO<List<PermissionDTO>>> permissionsUser()
        {
            var UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(UserId);
            List<PermissionDTO> permissions = await permissionService.permissionsUser(user);

            return new ResponseDTO<List<PermissionDTO>>(true, "success", permissions);
        }

        /// <summary>
        /// Endpoint para obtener todos los usuarios
        /// </summary>
        /// <returns> Una lista con los usuarios regitrados</returns>
        [HttpGet("getUsers")]
        public async Task<ResponseDTO<List<UserDTO>>> getUsers()
        {
            var users = _userManager.Users.ToList();
            List<UserDTO> data = new List<UserDTO>();
            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                data.Add(new UserDTO { Id = item.Id, UserName = item.FullName , Role = roles.FirstOrDefault(), Email = item.UserName, FullName = item.FullName});
            }

            return new ResponseDTO<List<UserDTO>>(true, "success", data);
        }

        /// <summary>
        /// Endpoint para obtener todos los usuarios con rol de Empleado
        /// </summary>
        /// <returns> Una lista con los empleados regitrados</returns>
        [HttpGet("getEmployees")]
        public async Task<ResponseDTO<List<UserDTO>>> getEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");
            List<UserDTO> employees = new List<UserDTO>();
            foreach (var user in users)
            {
                employees.Add(new UserDTO { Id = user.Id, UserName = user.FullName, Email = user.Email });
            }
            return new ResponseDTO<List<UserDTO>>(true, "succes", employees);
        }

        /// <summary>
        /// Endpoint para obtener todos los menus
        /// </summary>
        /// <returns> Una lista con los menus registrados</returns>
        [HttpGet("getMenus")]
        public ResponseDTO<List<MenuEntity>> getMenus()
        {
            var data = unitOfWork.Crud<MenuEntity>().GetAll().ToList();
            return new ResponseDTO<List<MenuEntity>>(true, "success", data);
        }

        /// <summary>
        /// Endpoint para obtener todos los menus asignados a un rol
        /// </summary>
        /// <param name="roleName">Rol del usuario</param>
        /// <returns> Una lista con los menus asignados a un rol</returns>
        [HttpGet("getMenusByRole")]
        public async Task<ResponseDTO<List<MenuDTO>>> GetMenusByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var menu = unitOfWork.Crud<MenuEntity>().GetAll();
            var permissions = unitOfWork.Crud<PermissionEntity>().Get(s => s.RoleId == role.Id);

            var data = (from m in menu
                        join p in permissions on m.MenuId equals p.MenuId into gj
                        from x in gj.DefaultIfEmpty()
                        select new MenuDTO
                        {
                            Id = m.MenuId,
                            Checked = x == null ? false : true,
                            Menu = m.Controller
                        }).ToList();

            return new ResponseDTO<List<MenuDTO>>(true, "success", data);
        }

        /// <summary>
        /// Endpoint para asignar un menu a un Rol
        /// </summary>
        /// <param name="menuRole">DTO con el menu y rol a asignarse</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost("assigMenuToRole")]
        public async Task<ResponseDTO> assigMenuToRole(MenuRoleDTO menuRole)
        {

            var role = await _roleManager.FindByNameAsync(menuRole.RoleName);
            unitOfWork.Crud<PermissionEntity>().DeleteRange(unitOfWork.Crud<PermissionEntity>().Get(s => s.RoleId == role.Id));
            unitOfWork.SaveChanges();
            List<PermissionEntity> permissions = new List<PermissionEntity>();
            foreach (var menu in menuRole.Menus)
            {
                permissions.Add(new PermissionEntity { RoleId = role.Id, MenuId = menu.Id, Read = true, Create = true, Update = true, Delete = true });
            }
            unitOfWork.Crud<PermissionEntity>().CreateRange(permissions);
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Permisos asignados correctamente");
        }

        /// <summary>
        /// Endpoint para agregar un menu
        /// </summary>
        /// <param name="menuName">Nombre del menu a adicionar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpGet("addMenu")]
        public ResponseDTO addMenu(string menuName)
        {
            var result = unitOfWork.Crud<MenuEntity>().Get(s => s.Controller == menuName).FirstOrDefault();
            if (result != null) return new ResponseDTO(false, "El menu ya existe");

            var menu = new MenuEntity() { Controller = menuName };
            unitOfWork.Crud<MenuEntity>().Create(menu);
            unitOfWork.SaveChanges();

            return new ResponseDTO(true, "Menu creado exitosamente.");
        }

        /// <summary>
        /// Endpoint para eliminar un menu por id
        /// </summary>
        /// <param name="menuId">Id del menu a eliminar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación</returns>
        [HttpGet("deleteMenu")]
        public ResponseDTO deleteMenu(int menuId)
        {
            var result = unitOfWork.Crud<PermissionEntity>().Get(s => s.MenuId == menuId).FirstOrDefault();
            if (result != null) return new ResponseDTO(false, "Este menu esta asociados a roles");

            unitOfWork.Crud<MenuEntity>().Delete(unitOfWork.Crud<MenuEntity>().Get(s => s.MenuId == menuId).First());
            unitOfWork.SaveChanges();

            return new ResponseDTO(true, "Menu eliminado correctamente");
        }

        /// <summary>
        /// Endpoint para eliminar un usuario por id
        /// </summary>
        /// <param name="id">Id del usuario a eliminar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpGet("deleteUser/{id}")]
        public ResponseDTO deleteUser(string id)
        {
            unitOfWork.Crud<AplicationUserEntity>().Delete(unitOfWork.Crud<AplicationUserEntity>().Get(s => s.Id == id).First());
            unitOfWork.SaveChanges();

            return new ResponseDTO(true, "Usuario elimando correctamente");
        }

        /// <summary>
        /// Endpoint para crear un usuario
        /// </summary>
        /// <param name="userCreateDTO">DTO con los parametros del usuario a crear</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost("createUser")]
        public async Task<ResponseDTO> createUser(UserCreateDTO userCreateDTO)
        {
            var user = new AplicationUserEntity { UserName = userCreateDTO.Username, Email = userCreateDTO.Username, FullName = userCreateDTO.FullName };

            var result = await _userManager.CreateAsync(user, userCreateDTO.Password);
            await _userManager.AddToRoleAsync(user, userCreateDTO.Role);

            return new ResponseDTO(result.Succeeded, result.Succeeded ? "Usuario creado exitosamente" : "Error creando el usuario");
        }

        /// <summary>
        /// Endpoint para actualizar un usuario
        /// </summary>
        /// <param name="userCreateDTO">DTO con los parametros del usuario a actualizar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost("updateUser")]
        public async Task<ResponseDTO> updateUser(UserCreateDTO userCreateDTO)
        {
            return await  loginService.updateUser(userCreateDTO);
        }
    }
}

