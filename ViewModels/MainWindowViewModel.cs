using LockAndKey.Database;
using LockAndKey.Helpers;
using LockAndKey.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LockAndKey.ViewModels
{
    internal class MainWindowViewModel
    {
        #region Private Variables

        private SqliteConnection _connection;
        private Authentication _authentication;
        private Boolean _databaseExists;
        private Boolean _userExists;
        private Content? _content = null;
        private Item? _selectedItem = null;

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

        internal Boolean DatabaseExists
        {
            get { return _databaseExists; }
            set { _databaseExists = value; }
        }

        internal Boolean UserExists
        {
            get { return _userExists; }
            set { _userExists = value; }
        }

        internal Content? Content
        {
            get { return _content; }
            set { _content = value; }
        }

        internal Item? SelectedItem 
        { 
            get { return _selectedItem; } 
            set { _selectedItem = value; } 
        }

        internal Dictionary<String, String> WebsiteList
        {
            get 
            {
                if (_content != null)
                {
                    return _content.Websites;
                }
                else return new Dictionary<String, String>();
            }
        }

        #endregion

        #region Constructor

        internal MainWindowViewModel()
        {
            var databaseLocation = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{Constants.DatabaseName}";
            _databaseExists = File.Exists(databaseLocation);
            if (DatabaseExists)
            {
                _connection = DatabaseDal.CreateDatabaseConnection(databaseLocation);
                _connection.Open();
                _authentication = new Authentication(_connection, Environment.UserName);
                _userExists = _authentication.UserExists;
            }
            else
            {
                _connection = DatabaseDal.CreateNewDatabase(databaseLocation);
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                _authentication = new Authentication(_connection, Environment.UserName);
                _userExists = false;
            }
        }

        #endregion

        #region Methods

        public void AuthenticateNewDatabase(String password)
        {
            if (_connection != null)
            {
                _authentication = new Authentication(_connection, Environment.UserName);
                _authentication.LoginUser(password);
            }
        }

        public void GetContent()
        {
            _content = new Content(_connection, _authentication);
        }

        public void SetSelectedItem(String itemName)
        {
            if (Content != null)
            {
                _selectedItem = Content.Items.Where(i => i.Name == itemName).First();
            }
        }

        #endregion
    }
}
