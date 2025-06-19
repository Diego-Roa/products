using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.DataAccess.Entities
{
    [Table("Permission")]
    public class PermissionEntity
    {
        [Key]
        public int PermissionId { get; set; }
        public bool Read { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

        [ForeignKey("MenuId")]
        public int MenuId { get; set; }
        public MenuEntity Menu { get; set; }

        [ForeignKey("Id")]
        public string RoleId { get; set; }
        public AplicationRoleEntity AplicationRole { get; set; }
    }
}
