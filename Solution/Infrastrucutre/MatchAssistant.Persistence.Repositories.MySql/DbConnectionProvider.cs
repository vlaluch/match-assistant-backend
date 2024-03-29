﻿using MySql.Data.MySqlClient;
using System.Data;

namespace MatchAssistant.Persistence.Repositories.MySql
{
    public class DbConnectionProvider : IDbConnectionProvider, System.IDisposable
    {
        private IDbConnection dbConnection;
        private readonly IDbConnectionStringProvider connectionStringDataProvider;

        public DbConnectionProvider(IDbConnectionStringProvider connectionStringDataProvider)
        {
            this.connectionStringDataProvider = connectionStringDataProvider;
        }

        public IDbConnection Connection
        {
            get
            {
                if (dbConnection == null)
                {
                    dbConnection = new MySqlConnection(connectionStringDataProvider.ConnectionString);
                }

                return dbConnection;
            }
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
