using System;

namespace LockAndKey.Models
{
    public class Item
    {
        public Int64 UserId { get; set; }
        public String Name { get; set; }
        public String Username { get; set; }
        public String Website { get; set; }
        public String Password { get; set; }
        public String Notes { get; set; }
        public String ItemKey { get; set; }

        public Item(Int64 userId, String name, String username, String website, String password, String notes)
        {
            UserId = userId;
            Name = name;
            Username = username;
            Website = website;
            Password = password;
            Notes = notes;
            ItemKey = $"{name}-{username}";
        }
    }
}
