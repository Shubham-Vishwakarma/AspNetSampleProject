using System;
using MySqlConnector;

namespace BuildRestApiNetCore.Database
{
    public class AppDatabase : IDisposable
    {
        public MySqlConnection Connection { get; }

        public AppDatabase(string dbConnectionString)
        {
            Connection = new MySqlConnection(dbConnectionString);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}