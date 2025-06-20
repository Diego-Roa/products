using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.DTOs
{
    public class ProductDTO
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public decimal UnitPrice { get; set; }

        public bool Status { get; set; }

        public string UnitMeasurement { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}