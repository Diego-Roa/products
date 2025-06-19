using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Services.Repositories;

namespace TaskManagement.Services.Services
{
    public class UnitOfWork
    {
        private readonly DbContext context;
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public GenericRepository<T> Crud<T>() where T : class
        {
            return new GenericRepository<T>(this.context);
        }


        public DbContext GetContext()
        {
            return context;
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

    }
}

