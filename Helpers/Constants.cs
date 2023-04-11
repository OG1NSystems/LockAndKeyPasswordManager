using System;

namespace LockAndKey.Helpers
{
    public static class Constants
    {
        #region Database Constants

        public const String DatabaseName = "manager.db";
        public const String DatabaseConnectionString = "Data Source={0};";
        public const String UserTableSqlString = "CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username NVARCHAR(256), PasswordHash BLOB, PasswordSalt BLOB, LockedOutDate NUMERIC, LockedOutCount INTEGER)";
        public const String DataTableSqlString = "CREATE TABLE Data (UserId INTEGER, Name NVARCHAR(256), Username NVARCHAR(256), Website NVARCHAR(1000), Password BLOB, Notes NVARCHAR(3000))";
        public const String InsertUserSqlString = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, LockedOutDate, LockedOutCount) VALUES (@username, @hash, @salt, null, 0)";
        public const String FetchUserSqlString = "SELECT * FROM Users WHERE Username = '{0}'";
        public const String UpdateUserLockedOutSqlString = "UPDATE Users SET LockedOutDate = @date, LockedOutCount = @count WHERE Username = '{0}'";
        public const String UpdateUserPasswordSqlString = "UPDATE Users SET PasswordHash = @hash, PasswordSalt = @salt WHERE Username = '{0}'";
        public const String FetchAllDataItemsString = "SELECT * FROM Data WHERE UserId = {0}";
        public const String InsertDataItemSqlString = "INSERT INTO Data (UserId, Name, Username, Website, Password, Notes) VALUES (@userID, @name, @username, @website, @password, @notes)";
        public const String DeleteDataItemString = "DELETE FROM Data WHERE UserId = {0} AND Name = '{1}'";
        public const String UpdateDataItemString = "UPDATE Data SET Username = @username, Website = @website, Password = @password, Notes = @notes WHERE UserId = {0} AND Name = '{1}'";

        #endregion

        #region Authentication Constants        

        public const String LockOutTimeMinute = "minute";
        public const String LockOutTimeMinutes = "minutes";
        public const String LockOutTimeHour = "hour";
        public const String LockOutTimeHours = "hours";
        public const String LockOutTimeDay = "day";

        public const String LoginMessageRetryAfterLockout = "You are still locked out, please wait {0} to try again.";
        public const String LoginMessageLockedOutTime = "You have been locked out for {0} {1}";
        public const String LoginMessageIncorrectDetails = "The {0} entered is incorrect please try again. You have {1} attempt(s) remaining.";
        public const String LoginMessageMaxRetryCount = "This database has now been completely locked down and can no longer be accessed.";
        public const String LoginMessageSuccessful = "Login Successful";

        #endregion

        #region Creation Constants

        public const String CreateAdminEmptyField = "All fields must have values.";
        public const String CreateAdminNotMatched = "The Password and Confirm Password do not match.";
        public const String CreateAdminInsufficientCount = "The password and the secret key must be at least 8 characters.";
        public const String CreateAdminMissingCharacterTypes = "The password must contain at least 1 uppercase letter, 1 lowercase letter, a number and a symbol.";
        public const String CreateSubTitle = "Create User Admin Details";
        public const String CreateInstructions = "Both the password and secret key must be at least 8 characters long, and the password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 symbol.";

        #endregion

        #region Login Constants

        public const String LoginEmptyPassword = "The password and secret key fields must have values";

        #endregion

        #region General

        public const String Warning = "Warning";
        public const String Information = "Information";
        public const String MainTitle = "Lock And Key";
        public const String PasswordString = "Password";
        public const String SecretKeyString = "Secret Key";



        #endregion

        #region ItemMessages

        public const String ItemDeleteSuccess = "{0} was deleted successfully";
        public const String ItemDeleteFailure = "The was an error deleting {0}, please try again.";
        public const String ItemSaveSuccess = "{0} was saved successfully.";
        public const String ItemSaveFailure = "There was an error saving {0}, please try again.";
        public const String ItemDuplicate = "There is already a item saved with that name, please choose a different name.";

        #endregion
    }
}
