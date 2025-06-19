namespace Tareas.DataAccess.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public User AssignedTo { get; set; }
    }
}