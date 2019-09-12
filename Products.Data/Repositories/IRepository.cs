using System.Collections.Generic;
using System.Data.SqlClient;

namespace Products.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        SqlConnection _connection { get; set; }

        IEnumerable<T> GetAll();

        IEnumerable<T> Search(string name);

        T Get(int id);

        T Add(T entity);

        T Update(int id, T updates);

        void Delete(int id);
    }
}
