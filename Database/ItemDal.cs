using LockAndKey.Helpers;
using LockAndKey.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace LockAndKey.Database
{
    public class ItemDal
    {
        public static List<Item> FetchDataItems(SqliteConnection connection, Int64 userId, Byte[] secret, Byte[] iv)
        {
            List<Item> items = new List<Item>();
            using (SqliteCommand fetchAllDataItemsCommand = new(Constants.FetchAllDataItemsString, connection))
            {
                fetchAllDataItemsCommand.Parameters.Add(new SqliteParameter("userID", userId));
                SqliteDataReader reader = fetchAllDataItemsCommand.ExecuteReader();
                while (reader.Read())
                {
                    var decryptedPassword = CryptoHelper.DecryptPasswordValue((Byte[])reader["Password"], secret, iv);
                    var item = new Item(
                        (Int64)reader["UserId"],
                        (String)reader["Name"],
                        (String)reader["Username"],
                        (String)reader["Website"],
                        decryptedPassword,
                        (String)reader["Notes"]
                        );
                    items.Add(item);
                }
            }
            return items;
        }

        internal static void InsertNewItem(SqliteConnection connection, Item item, Authentication authentication)
        {
            var encryptedPassword = CryptoHelper.EncryptPasswordValue(item.Password, authentication.SecretKey, authentication.SecretIV);
            using (SqliteCommand createItemCommand = new(Constants.InsertDataItemSqlString, connection))
            {
                createItemCommand.Parameters.Add(new SqliteParameter("userID", item.UserId));
                createItemCommand.Parameters.Add(new SqliteParameter("name", item.Name));
                createItemCommand.Parameters.Add(new SqliteParameter("username", item.Username));
                createItemCommand.Parameters.Add(new SqliteParameter("website", item.Website));
                createItemCommand.Parameters.Add(new SqliteParameter("password", encryptedPassword));
                createItemCommand.Parameters.Add(new SqliteParameter("notes", item.Notes));
                createItemCommand.ExecuteNonQuery();
            }
        }

        internal static void UpdateExistingItem(SqliteConnection connection, Item item, Authentication authentication)
        {
            var encryptedPassword = CryptoHelper.EncryptPasswordValue(item.Password, authentication.SecretKey, authentication.SecretIV);
            using (SqliteCommand updateItemCommand = new(Constants.UpdateDataItemString, connection))
            {
                updateItemCommand.Parameters.Add(new SqliteParameter("userID", item.UserId));
                updateItemCommand.Parameters.Add(new SqliteParameter("name", item.Name));
                updateItemCommand.Parameters.Add(new SqliteParameter("username", item.Username));
                updateItemCommand.Parameters.Add(new SqliteParameter("website", item.Website));
                updateItemCommand.Parameters.Add(new SqliteParameter("password", encryptedPassword));
                updateItemCommand.Parameters.Add(new SqliteParameter("notes", item.Notes));
                updateItemCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteItem(Item item, SqliteConnection connection)
        {
            using (SqliteCommand deleteItemCommand = new(Constants.DeleteDataItemString, connection)) 
            {
                deleteItemCommand.Parameters.Add(new SqliteParameter("userID", item.UserId));
                deleteItemCommand.Parameters.Add(new SqliteParameter("name", item.Name));
                deleteItemCommand.ExecuteNonQuery();
            }
        }
    }
}
