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
using AutoMapper;
using System.Data;

namespace TaskManagement.Services.Services
{
    public class ProductsService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public ProductsService(UnitOfWork unitOfWork, ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Crear Producto.
        /// </summary>
        /// <returns>Mensaje con exito o fracaso de la operación</returns>
        public ResponseDTO createProduct(ProductDTO productDTO)
        {
            var product = new ProductsEntity()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Reference = productDTO.Reference,
                UnitPrice = productDTO.UnitPrice,
                Status = productDTO.Status,
                UnitMeasurement = productDTO.UnitMeasurement,
                CreatedAt = productDTO.CreatedAt,
            };
            unitOfWork.Crud<ProductsEntity>().Create(product);
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Producto creado exitosamente");
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Mensaje con exito o fracaso de la operación</returns>
        public ResponseDTO<List<ProductDTO>> getProducts()
        {
            var products = new List<ProductDTO>();
            products = _mapper.Map<List<ProductDTO>>(unitOfWork.Crud<ProductsEntity>().GetAll().ToList());
            return new ResponseDTO<List<ProductDTO>>(true, "Productos obtenidos exitosamente", products);
        }

        /// <summary>
        /// Actualizar un producto Producto.
        /// </summary>
        /// <returns>Mensaje con exito o fracaso de la operación</returns>
        public ResponseDTO updateProduct(int id, ProductDTO productDTO)
        {
            var product = new ProductsEntity()
            {
                Id = id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Reference = productDTO.Reference,
                UnitPrice = productDTO.UnitPrice,
                Status = productDTO.Status,
                UnitMeasurement = productDTO.UnitMeasurement,
                CreatedAt = productDTO.CreatedAt,
            };
            unitOfWork.Crud<ProductsEntity>().Update(product);
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Producto actualizado exitosamente");
        }
        
        /// <summary>
        /// Elimina un producto por su id
        /// </summary>
        /// <param name="id"->Id del producto a eliminar </param>
        public ResponseDTO deleteProduct(int id)
        {
            unitOfWork.Crud<ProductsEntity>().Delete(unitOfWork.Crud<ProductsEntity>().Get(s => s.Id == id).First());
            unitOfWork.SaveChanges();
            return new ResponseDTO(true, "Producto eliminado exitosamente");
        }
    }
}