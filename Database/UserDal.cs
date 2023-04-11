using LockAndKey.Helpers;
using LockAndKey.Models;
using Microsoft.Data.Sqlite;
using System;

namespace LockAndKey.Database
{
    public class UserDal
    {
        public static void CreateNewUser(SqliteConnection connection, String username, String password)
        {
            CryptoHelper.CreateHashAndSalt(password, out Byte[] hash, out Byte[] salt);
            using (SqliteCommand createUserCommand = new SqliteCommand(Constants.InsertUserSqlString, connection))
            {
                createUserCommand.Parameters.Add(new SqliteParameter("username", username));
                createUserCommand.Parameters.Add(new SqliteParameter("hash", hash));
                createUserCommand.Parameters.Add(new SqliteParameter("salt", salt));
                createUserCommand.ExecuteNonQuery();
            }
        }

        public static User GetCurrentUser(SqliteConnection connection, String username)
        {
            var fetchString = String.Format(Constants.FetchUserSqlString, username);
            using (SqliteCommand fetchUserCommand = new(fetchString, connection))
            {
                SqliteDataReader reader = fetchUserCommand.ExecuteReader();
                if (reader.Read())
                {
                    var lockedOutDate = Convert.IsDBNull(reader["LockedOutDate"]) ? null : (DateTime?)reader["LockedOutDate"];
                    return new User(
                        (Int64)reader["Id"],
                        (String)reader["Username"],
                        (Byte[])reader["PasswordHash"],
                        (Byte[])reader["PasswordSalt"],
                        lockedOutDate,
                        Convert.ToInt32(reader["LockedOutCount"]));
                }
                else
                {
                    return new User(0,
                                    String.Empty,
                                    Array.Empty<Byte>(),
                                    Array.Empty<Byte>());
                }
            }
        }

        public static void UpdateLockedOutUser(SqliteConnection connection, String username, Int32 lockedOutCount)
        {
            DateTime? date = lockedOutCount > 0 ? DateTime.UtcNow : null;
            var updateString = String.Format(Constants.UpdateUserLockedOutSqlString, date, lockedOutCount, username);
            using (SqliteCommand updateUserCommand = new(updateString, connection))
            {
                updateUserCommand.ExecuteNonQuery();
            }
        }
    }
}
