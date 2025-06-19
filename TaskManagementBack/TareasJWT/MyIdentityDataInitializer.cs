using System.Linq;
using Microsoft.AspNetCore.Identity;
using TaskManagement.DataAccess;
using TaskManagement.DataAccess.Entities;

namespace TaskManagement
{
     public static class MyIdentityDataInitializer
    {

        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AplicationUserEntity>>();
            var roleManager = serviceProvider.GetService<RoleManager<AplicationRoleEntity>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();


            SeedRoles(roleManager, userManager);
            SeedUsers(userManager);
            SeedPermissions(context, roleManager, userManager);
        }

        public static void SeedUsers(UserManager<AplicationUserEntity> userManager)
        {
            if (!userManager.Users.Any(u => u.UserName == "sebas@gmail.com"))
            {
                var user = new AplicationUserEntity();
                user.UserName = "sebas@gmail.com";
                user.Email = "sebas@gmail.com";
                user.FullName = "Sebastian Roa";

                var result = userManager.CreateAsync(user, "123456*Abc").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<AplicationRoleEntity> roleManager, UserManager<AplicationUserEntity> userManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new AplicationRoleEntity();
                role.Name = "Administrator";
                var roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Supervisor").Result)
            {
                var role = new AplicationRoleEntity();
                role.Name = "Supervisor";
                var roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new AplicationRoleEntity();
                role.Name = "Employee";
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedPermissions(ApplicationDbContext context, RoleManager<AplicationRoleEntity> roleManager, UserManager<AplicationUserEntity> userManager) 
        {
            var task = new MenuEntity() { Controller = "/tasks" };
            var users = new MenuEntity() { Controller = "/users" };

            if (context.Permissions.ToList().Count == 0)
            {
                context.Menus.Add(task);
                context.SaveChanges();

                context.Menus.Add(users);
                context.SaveChanges();


                //roles para administrador
                var role_administrator = roleManager.FindByNameAsync("Administrator").Result;
                context.Permissions
                    .Add(new PermissionEntity() { Update = true, Read = true, Delete = true, Create = true, MenuId = task.MenuId, RoleId = role_administrator.Id  });
                
                context.Permissions
                    .Add(new PermissionEntity() { Update = true, Read = true, Delete = true, Create = true, MenuId = users.MenuId, RoleId = role_administrator.Id });
                context.SaveChanges();

                //roles para supervisor
                var role_supervisor = roleManager.FindByNameAsync("Supervisor").Result;
                context.Permissions
                    .Add(new PermissionEntity() { Update = true, Read = true, Delete = true, Create = true, MenuId = task.MenuId, RoleId = role_supervisor.Id });

                context.Permissions
                    .Add(new PermissionEntity() { Update = false, Read = false, Delete = false, Create = false, MenuId = users.MenuId, RoleId = role_supervisor.Id });
                context.SaveChanges();


                //roles para empleado
                var role_employee = roleManager.FindByNameAsync("Employee").Result;
                context.Permissions
                    .Add(new PermissionEntity() { Update = true, Read = true, Delete = false, Create = false, MenuId = task.MenuId, RoleId = role_employee.Id });

                context.Permissions
                    .Add(new PermissionEntity() { Update = false, Read = false, Delete = false, Create = false, MenuId = users.MenuId, RoleId = role_employee.Id });
                context.SaveChanges();
            }
        }
    }
}