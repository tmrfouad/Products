using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Products.Data.Repositories
{
    public abstract class RepositoryBase
    {
        public SqlConnection connection { get; set; }

        public RepositoryBase()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("ProductsDatabase");
            connection = new SqlConnection(connectionString);
        }
    }
}
