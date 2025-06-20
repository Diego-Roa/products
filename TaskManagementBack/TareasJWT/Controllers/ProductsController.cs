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
    public class ProductsController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly ProductsService productsService;

        public ProductsController(UnitOfWork unitOfWork, ProductsService productsService)
        {
            this.unitOfWork = unitOfWork;
            this.productsService = productsService;
        }

        /// <summary>
        /// Endpoint para crear un producto
        /// </summary>
        /// <param name="productDTO">DTO con los parametros de la tarea a crear</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost()]
        public async Task<ResponseDTO> createProduct([FromBody] ProductDTO productDTO)
        {
            return productsService.createProduct(productDTO);
        }
        
        /// <summary>
        /// Endpoint para obtener las tareas segun el rol del usuario
        /// </summary>
        /// <returns> Lista de tareas por usuario</returns>
        [HttpGet()]
        public ResponseDTO<List<ProductDTO>> getTasks()
        {
            return productsService.getProducts();
        }
    }
}
