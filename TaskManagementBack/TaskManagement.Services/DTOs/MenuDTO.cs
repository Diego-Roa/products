using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.DTOs
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string Menu { get; set; }
    }
}
