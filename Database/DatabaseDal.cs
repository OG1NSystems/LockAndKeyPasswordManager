using LockAndKey.Helpers;
using Microsoft.Data.Sqlite;
using System;

namespace LockAndKey.Database
{
    internal class DatabaseDal
    {
        internal static SqliteConnection CreateNewDatabase(String databaseLocation)
        {
            SqliteConnection connection = CreateDatabaseConnection(databaseLocation);
            connection.Open();
            CreateDatabaseTables(connection);
            return connection;
        }

        internal static SqliteConnection CreateDatabaseConnection(String databaseLocation)
        {
            var connectionString = String.Format(Constants.DatabaseConnectionString, databaseLocation);
            return new SqliteConnection(connectionString);
        }

        private static void CreateDatabaseTables(SqliteConnection connection)
        {
            //Creates a user table
            using (SqliteCommand userTableCommand = new(Constants.UserTableSqlString, connection))
            {
                userTableCommand.ExecuteNonQuery();
            }

            //Creates a data table
            using (SqliteCommand dataTableCommand = new(Constants.DataTableSqlString, connection))
            {
                dataTableCommand.ExecuteNonQuery();
            }
        }
    }
}
