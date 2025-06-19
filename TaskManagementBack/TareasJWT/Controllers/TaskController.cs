using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Services.Services;
using TaskManagement.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.Controllers
{
    /// <summary>
    /// Controlador REST para gestionar tareas
    /// Proporciona endpoints para obtener, crear, editar y eliminar tareas
    /// 
    /// <p> Autor: Sebastian Roa </p>
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly TaskService taskService;

        public TaskController(UnitOfWork unitOfWork, TaskService taskService)
        {
            this.unitOfWork = unitOfWork;
            this.taskService = taskService;
        }

        /// <summary>
        /// Endpoint para crear una tarea
        /// </summary>
        /// <param name="taskDTO">DTO con los parametros de la tarea a crear</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost("createTask")]
        public async Task<ResponseDTO> createTask(TaskDTO taskDTO) 
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return taskService.createTask(taskDTO, usuarioId);
        }

        /// <summary>
        /// Endpoint para actualizar una tarea
        /// </summary>
        /// <param name="taskDTO">DTO con los parametros de la tarea a actualizar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost("updateTask")]
        public async Task<ResponseDTO> updateTask(TaskDTO taskDTO)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return taskService.updateTask(taskDTO, usuarioId);
        }

        /// <summary>
        /// Endpoint para eliminar una tarea por el id
        /// </summary>
        /// <param name="id">Id de la tarea a eliminar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpGet("deleteTask/{id}")]
        public ResponseDTO deleteTask(int id)
        {
            return taskService.deleteTask(id);
        }

        /// <summary>
        /// Endpoint para obtener las tareas segun el rol del usuario
        /// </summary>
        /// <returns> Lista de tareas por usuario</returns>
        [HttpGet("getTasks")]
        public async Task<ResponseDTO<List<TaskDTO>>> getTasks()
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await taskService.GetTask(usuarioId);
        }

        /// <summary>
        /// Endpoint para obtener los permisos de un usuario por su id
        /// </summary>
        /// <returns> permisos de un usario por su Id</returns>
        [HttpGet("getPermissionByUserId")]
        public async Task<ResponseDTO<object>> getPermissionsByUser()
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await taskService.GetPermissions(usuarioId);
        }
    }
}
