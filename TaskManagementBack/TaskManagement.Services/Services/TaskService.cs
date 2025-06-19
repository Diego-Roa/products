using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Services.DTOs;
using TaskManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TaskManagement.DataAccess;
using System.Data;

namespace TaskManagement.Services.Services
{
    /// <summary>
    /// Servicio para gestionar operaciones relacionadas con las tareas.
    /// </summary>
    public class TaskService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly RoleManager<AplicationRoleEntity> _roleManager;
        private readonly UserManager<AplicationUserEntity> _userManager;
        private readonly ApplicationDbContext context;

        public TaskService(UnitOfWork unitOfWork, RoleManager<AplicationRoleEntity> _roleManager,
            UserManager<AplicationUserEntity> _userManager, ApplicationDbContext context) 
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }

        /// <summary>
        /// Crea la tarea.
        /// </summary>
        /// <returns>Mensaje con exito o fracaso de la operación</returns>
        public ResponseDTO createTask(TaskDTO taskDTO, string usuarioId)
        {
            DateTime dateTime = DateTime.Now;
            var task = new TaskManagementEntity()
            {
                CreatedDate = dateTime,
                UpdatedDate = dateTime,
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Status = taskDTO.Status != null ? taskDTO.Status : "Pendiente",
                Executor = taskDTO.EmployeeId,
                Supervisor = usuarioId
            };
            unitOfWork.Crud<TaskManagementEntity>().Create(task);
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Tarea creada exitosamente");
        }

        /// <summary>
        /// Elimina un tarea por su id
        /// </summary>
        /// <param name="id"->Id de la tarea a eliminar </param>
        public ResponseDTO deleteTask(int id)
        {
            unitOfWork.Crud<TaskManagementEntity>().Delete(unitOfWork.Crud<TaskManagementEntity>().Get(s => s.TaskId == id).First());
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Tarea eliminada exitosamente");
        }

        /// <summary>
        /// Obtiene los permisos a los que tiene un usuario segun su rol
        /// </summary>
        /// <returns> Objeto con los permisos y menus a los que el usuario tiene acceso</returns>
        public async Task<ResponseDTO<object>> GetPermissions(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(s => s.UserName == userId);
            var roles = await _userManager.GetRolesAsync(user);
            var roleEntity =  _roleManager.FindByNameAsync(roles.First()).Result;

            var permissions = (from p in context.Permissions
                               join m in context.Menus on p.MenuId equals m.MenuId
                               where   (p.RoleId == roleEntity.Id)
                                select  new
                                {
                                    _Read = p.Read,
                                    _Create = p.Create,
                                    _Update = p.Update,
                                    _Delete = p.Delete,
                                    Menu = m.Controller
                                }
                               ).ToList();
              return new ResponseDTO<object>(true, "success", permissions);
        }

        /// <summary>
        /// Obtiene las tareas segun el rol y id del usuario
        /// </summary>
        /// <returns> Lista de tareas </returns>
        public async Task<ResponseDTO<List<TaskDTO>>> GetTask(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(s => s.UserName == userId);
            var roles = await _userManager.GetRolesAsync(user);
            var taskResponse = new ResponseDTO<List<TaskDTO>>(true, "success", null);
            if (roles.FirstOrDefault() != null)
            {
                var role = roles.First();
                var tasks = unitOfWork.Crud<TaskManagementEntity>()
                            .Get(s =>
                                (role == "Administrator" || role == "Supervisor" || s.Executor == user.Id)
                            ).ToList();
                var users = unitOfWork.Crud<AplicationUserEntity>().GetAll();

                var data = (from m in tasks
                            join p in users on m.Executor equals p.Id into gj
                            from x in gj.DefaultIfEmpty()
                            select new TaskDTO
                            {
                                EmployeeId = x.Id,
                                EmployeeName = x.FullName,
                                Id = m.TaskId,
                                Name = m.Name,
                                Status = m.Status,
                                Description = m.Description,
                                Supervisor = m.Supervisor,
                                CreatedDate = m.CreatedDate
                            }).ToList();
                taskResponse.Data = data;
            }
            return taskResponse;
        }

        public ResponseDTO updateTask(TaskDTO taskDTO, string usuarioId)
        {
            DateTime dateTime = DateTime.Now;
            var task = new TaskManagementEntity()
            {
                TaskId = taskDTO.Id,
                UpdatedDate = dateTime,
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Status = taskDTO.Status != null ? taskDTO.Status : "Pendiente",
                Executor = taskDTO.EmployeeId,
                Supervisor = taskDTO.Supervisor,
                CreatedDate = dateTime
            };
            unitOfWork.Crud<TaskManagementEntity>().Update(task);
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Tarea actualizada exitosamente");
        }
    }
}
