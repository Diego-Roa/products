using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.DTOs
{
    public class PermissionDTO
    {
        public string Menu { get; set; }
        public string[] Permissions { get; set; }
    }
}
