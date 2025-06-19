using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.DataAccess.Entities
{
    [Table("Menu")]
    public class MenuEntity
    {
        [Key]
        public int MenuId { get; set; }
        public string Controller { get; set; }
    }
}