using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.Interfaces
{

    public interface ICrud<T>
    {

        T FindBy(Func<T, bool> predicate);

        Task<T> FindAsync(params object[] keyValues);

        Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> Get(Func<T, bool> predicate);

        void Create(T entity);

        void CreateRange(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        Task DeleteAsync(params object[] id);

        bool Exists(Func<T, bool> predicate);

        int Count();
    }
}
