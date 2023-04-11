using LockAndKey.Helpers;
using LockAndKey.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Security.Cryptography;
using System.Text;

namespace LockAndKey.Database
{
    internal class Authentication
    {
        private SqliteConnection _connection;
        private Int64 _retryCount;
        private User _currentUser;
        private Boolean _userExists;
        private String _loginMessage = String.Empty;
        private Boolean _isLockedOut;
        private Byte[] _secretKey = new Byte[0];
        private Byte[] _secretIV;
        private Boolean _isLoggedIn = false;

        #region Properties

        internal Int64 RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }
        internal User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        internal Boolean UserExists
        {
            get { return _userExists; }
            set { _userExists = value; }
        }
        internal String LoginMessage
        {
            get { return _loginMessage; }
            set { _loginMessage = value; }
        }
        internal Boolean IsLockedOut
        {
            get { return _isLockedOut; }
            set { _isLockedOut = value; }
        }
        internal Byte[] SecretKey
        {
            get { return _secretKey; }
            set { _secretKey = value; }
        }
        internal Byte[] SecretIV
        {
            get { return _secretIV; }
            set { _secretIV = value; }
        }
        internal Boolean IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

        #endregion

        #region Constructor

        internal Authentication(SqliteConnection connection, String username)
        {
            _connection = connection;
            _retryCount = 0;
            _currentUser = UserDal.GetCurrentUser(connection, username);
            _userExists = _currentUser.Id > 0;
            _secretIV = CryptoHelper.CreateIV(username);
            if (_userExists)
            {
                _isLockedOut = IsUserLockedOut();
            }
        }

        #endregion

        #region Methods        

        internal Boolean IsUserLockedOut()
        {
            if (_currentUser.LockedOutDate != null)
            {
                var timeRemainingMessage = AuthenticationHelper.GetLockOutTimeRemainingString((DateTime)_currentUser.LockedOutDate, _currentUser.LockedOutCount);
                if (!String.IsNullOrEmpty(timeRemainingMessage))
                {
                    _loginMessage = String.Format(Constants.LoginMessageRetryAfterLockout, timeRemainingMessage);
                    return true;
                }
                else
                {
                    _currentUser.LockedOutDate = null;
                }
            }
            return false;
        }

        internal Boolean LoginUser(String password)
        {
            _secretKey = CryptoHelper.CreateKey(password);
            while (_retryCount < 10 && _currentUser.LockedOutCount < 6)
            {
                using (var hmac = new HMACSHA512(_currentUser.Salt))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < hash.Length; i++)
                    {
                        if (hash[i] != CurrentUser.Hash[i])
                        {
                            _retryCount++;
                            _loginMessage = String.Format(Constants.LoginMessageIncorrectDetails, Constants.PasswordString, _retryCount);
                            _isLoggedIn = false;
                            return false;
                        }
                    }
                }
                if (_currentUser.LockedOutCount > 0)
                {
                    UserDal.UpdateLockedOutUser(_connection, _currentUser.Name, 0);
                }
                _retryCount = 0;
                _loginMessage = Constants.LoginMessageSuccessful;
                _isLoggedIn = true;
                return true;
            }
            if (_currentUser.LockedOutCount < 5)
            {
                _currentUser.LockedOutCount++;
                _loginMessage = AuthenticationHelper.SetLockOutMessage(_currentUser.LockedOutCount);
                UserDal.UpdateLockedOutUser(_connection, _currentUser.Name, _currentUser.LockedOutCount);
            }
            else
            {
                _loginMessage = Constants.LoginMessageMaxRetryCount;
            }
            _isLockedOut = true;
            _isLoggedIn = false;
            return false;
        }

        #endregion
    }
}
