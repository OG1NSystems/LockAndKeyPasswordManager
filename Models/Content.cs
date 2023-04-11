using LockAndKey.Database;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LockAndKey.Models
{
    public class Content
    {
        internal List<Item> Items { get; set; }
        public Dictionary<String, String> Websites { get; set; }

        internal Content(SqliteConnection connection, Authentication authentication)
        {
            Items = ItemDal.FetchDataItems(connection, authentication.CurrentUser.Id, authentication.SecretKey, authentication.SecretIV);
            Websites = GetWebsiteDictionary();
        }

        private Dictionary<String, String> GetWebsiteDictionary()
        {
            var websiteDictionary = new Dictionary<String, String>();
            var orderedItems = Items.OrderBy(i => i.Name).ToList();
            foreach (var item in orderedItems)
            {
                websiteDictionary.Add(item.Name, item.Website);
            }
            return websiteDictionary;
        }
    }
}
