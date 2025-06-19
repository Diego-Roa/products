using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.DTOs
{
    public class MenuRoleDTO
    {
        public string RoleName { get; set; }
        public MenuDTO[] Menus { get; set; }
    }
}
