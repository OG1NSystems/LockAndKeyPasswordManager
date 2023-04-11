using LockAndKey.Database;
using LockAndKey.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockAndKey.ViewModels
{
    internal class MainContentViewModel
    {
        #region Fields

        private SqliteConnection _connection;
        private Authentication _authentication;
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
        public Content? Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public Item? SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        public Dictionary<String, String> WebsiteList
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

        internal MainContentViewModel(SqliteConnection connection, Authentication authentication)
        {
            _connection = connection;
            _authentication = authentication;
            GetContent();
        }

        #endregion

        #region Methods

        public void GetContent()
        {
            _content = new Content(_connection, _authentication);
            if (_selectedItem != null)
            {
                var selectedItem = _content.Items.Where(w => w.Name == _selectedItem.Name).FirstOrDefault();
                if (selectedItem != null)
                {
                    _selectedItem = selectedItem;
                }
            }
        }

        public void SetSelectedItem(String itemName)
        {
            if (_content != null)
            {
                _selectedItem = _content.Items.Where(i => i.Name == itemName).First();
            }
        }

        internal void SaveItem(String name, String username, String website, String password, String notes)
        {
            if (_selectedItem == null)
            {
                var itemToBeSaved = new Item(_authentication.CurrentUser.Id, name, username, website, password, notes);
                ItemDal.InsertNewItem(_connection, itemToBeSaved, _authentication);
                _selectedItem = itemToBeSaved;
            }
            else
            {
                ItemDal.UpdateExistingItem(_connection, _selectedItem, _authentication);
            }
            GetContent();
        }

        internal void DeleteItem()
        {
            if ( _selectedItem != null )
            {
                ItemDal.DeleteItem(_selectedItem, _connection);
                _selectedItem = null;
                GetContent();
            }
        }

        #endregion
    }
}
