using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<AplicationUserEntity, AplicationRoleEntity, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets adicionales si es necesario
        public DbSet<MenuEntity> Menus { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<TaskManagementEntity> Task { get; set; }
        public DbSet<ProductsEntity> Products { get; set; }
        public DbSet<AplicationUserEntity> Users { get; set; }

    }
}
