using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DataAccess.Entities;
using TaskManagement.Services.DTOs;

namespace TaskManagement.Services.Services
{
    public class PermissionService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly RoleManager<AplicationRoleEntity> roleManager;
        private readonly UserManager<AplicationUserEntity> userManager;

        public PermissionService(UnitOfWork unitOfWork, RoleManager<AplicationRoleEntity> roleManager,
        UserManager<AplicationUserEntity> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<bool> canQuery(string roleName, string route)
        {

            var role = await roleManager.FindByNameAsync(roleName);
            var resources = unitOfWork.Crud<MenuEntity>().Get(r => r.Controller == route);
            var permissions = unitOfWork.Crud<PermissionEntity>().GetAll();

            return resources.Join(permissions, r => r.MenuId, p => p.MenuId, (r, p) => p)
                    .Any(p => p.Read == true && p.RoleId == role.Id);
        }

        public async Task<bool> canDelete(string roleName, string route)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var resources = unitOfWork.Crud<MenuEntity>().Get(r => r.Controller == route);
            var permissions = unitOfWork.Crud<PermissionEntity>().GetAll();

            return resources.Join(permissions, r => r.MenuId, p => p.MenuId, (r, p) => p)
                    .Any(p => p.Delete == true && p.RoleId == role.Id);
        }

        public async Task<bool> canUpdate(string roleName, string route)
        {

            var role = await roleManager.FindByNameAsync(roleName);
            var resources = unitOfWork.Crud<MenuEntity>().Get(r => r.Controller == route);
            var permissions = unitOfWork.Crud<PermissionEntity>().GetAll();

            return resources.Join(permissions, r => r.MenuId, p => p.MenuId, (r, p) => p)
                    .Any(p => p.Update == true && p.RoleId == role.Id);
        }

        public async Task<bool> canCreate(string roleName, string route)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var resources = unitOfWork.Crud<MenuEntity>().Get(r => r.Controller == route);
            var permissions = unitOfWork.Crud<PermissionEntity>().GetAll();

            return resources.Join(permissions, r => r.MenuId, p => p.MenuId, (r, p) => p)
                    .Any(p => p.Create == true && p.RoleId == role.Id);
        }

        public async Task<List<PermissionDTO>> permissionsUser(AplicationUserEntity user)
        {
            var roleNames = await userManager.GetRolesAsync(user);
            var roleName = roleNames.FirstOrDefault();
            if (roleName == null) return null;
            var role = await roleManager.FindByNameAsync(roleName);

            var result = unitOfWork.Crud<MenuEntity>().GetAll()
                        .Join(unitOfWork.Crud<PermissionEntity>().Get(p => p.RoleId == role.Id),
                            m => m.MenuId,
                            p => p.MenuId,
                            (m, p) => new PermissionDTO
                            {
                                Menu = m.Controller,
                                Permissions = new List<string> { p.Read ? "READ" : "NONE", p.Create ? "CREATE" : "NONE", p.Update ? "UPDATE" : "NONE", p.Delete ? "DELETE" : "NONE" }.ToArray()
                            }).ToList();
            return result;
        }
    }
}