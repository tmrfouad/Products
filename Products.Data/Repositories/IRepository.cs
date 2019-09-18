using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        SqlConnection _connection { get; set; }

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> Search(string name);

        Task<T> Get(int id);

        Task<T> Add(T entity);

        Task<T> Update(int id, T updates);

        Task<bool> Delete(int id);
    }
}
