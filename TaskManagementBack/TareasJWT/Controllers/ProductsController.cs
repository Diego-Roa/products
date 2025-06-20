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
    /// <remarks>
    /// Autor: Sebastian Roa
    /// </remarks>
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductsService productsService;

        public ProductsController(ProductsService productsService)
        {
            this.productsService = productsService;
        }

        /// <summary>
        /// Endpoint para crear un producto
        /// </summary>
        /// <param name="productDTO">DTO con los parametros de la tarea a crear</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPost()]
        public ResponseDTO CreateProduct([FromBody] ProductDTO productDTO)
        {
            return productsService.createProduct(productDTO);
        }

        /// <summary>
        /// Endpoint para obtener los productos
        /// </summary>
        /// <returns> Lista de productos</returns>
        [HttpGet()]
        public ResponseDTO<List<ProductDTO>> GetProducts()
        {
            return productsService.getProducts();
        }

        /// <summary>
        /// Endpoint para actualizar un producto
        /// </summary>
        /// <param name="productDTO">DTO con los parametros del producto a actualizar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpPut("{id}")]
        public ResponseDTO UpdateProduct([FromRoute] int id, [FromBody] ProductDTO productDTO)
        {
            return productsService.updateProduct(id, productDTO);
        }

        /// <summary>
        /// Endpoint para eliminar un producto
        /// </summary>
        /// <param name="productDTO">DTO con los parametros del producto a actualizar</param>
        /// <returns> Mensaje con el exito o fracaso de la operación.</returns>
        [HttpDelete("{id}")]
        public ResponseDTO DeleteProduct([FromRoute] int id)
        {
            return productsService.deleteProduct(id);
        }
    }
}
