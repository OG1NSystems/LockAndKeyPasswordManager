using System;

namespace LockAndKey.Models
{
    public class User
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Byte[] Hash { get; set; }
        public Byte[] Salt { get; set; }
        public DateTime? LockedOutDate { get; set; }
        public Int32 LockedOutCount { get; set; }

        public User(Int64 id, String name, Byte[] hash, Byte[] salt, DateTime? lockedOutDate = null, Int32 lockedOutCount = 0)
        {
            Id = id;
            Name = name;
            Hash = hash;
            Salt = salt;
            LockedOutDate = lockedOutDate;
            LockedOutCount = lockedOutCount;
        }
    }
}
