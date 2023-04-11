using LockAndKey.Database;
using Microsoft.Data.Sqlite;
using System;

namespace LockAndKey.ViewModels
{
    internal class LoginUserControlViewModel
    {
        private SqliteConnection _connection;
        private Authentication _authentication;

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

        #endregion

        #region Constructor

        internal LoginUserControlViewModel(SqliteConnection connection, Authentication authentication)
        {
            _connection = connection;
            _authentication = authentication;
        }

        #endregion

        #region Methods

        internal Boolean LoginUser(String password)
        {
            return _authentication.LoginUser(password);
        }

        #endregion
    }
}
