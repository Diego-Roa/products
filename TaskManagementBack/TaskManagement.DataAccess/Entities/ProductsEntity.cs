using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DataAccess.Entities
{
    [Table("Products")]
    public class ProductsEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public decimal UnitPrice { get; set; }

        public bool Status { get; set; }

        public string UnitMeasurement { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}