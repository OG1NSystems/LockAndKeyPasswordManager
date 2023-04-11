using LockAndKey.Database;
using Microsoft.Data.Sqlite;
using System;

namespace LockAndKey.ViewModels
{
    internal class CreateUserControlViewModel
    {
        #region Fields

        private SqliteConnection _connection;
        private Authentication _authentication;
        private String _password;

        #endregion

        #region Properties

        internal SqliteConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
        internal Authentication Authentication
        { 
            get { return _authentication; } 
            set { _authentication = value; }
        }
        internal String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        internal CreateUserControlViewModel(SqliteConnection connection, Authentication authentication)
        {
            _connection = connection;
            _authentication = authentication;
            _password = String.Empty;
        }

        internal void CreateUser(String password)
        {
            try
            {
                UserDal.CreateNewUser(_connection, Environment.UserName, password);
                _password = password;
                _authentication.CurrentUser = UserDal.GetCurrentUser(_connection, Environment.UserName);
                _authentication.LoginUser(password);
            }
            catch { }
        }
    }
}
