using LockAndKey.Database;
using LockAndKey.Helpers;
using LockAndKey.ViewModels;
using Microsoft.Data.Sqlite;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LockAndKey.Views
{
    /// <summary>
    /// Interaction logic for MainContentUserControl.xaml
    /// </summary>
    public partial class MainContentUserControl : UserControl
    {
        #region Constants

        private const String _deleteButton = "DELETE";
        private const String _saveButton = "SAVE";

        private NotificationManager _notificationManager;

        #endregion

        #region Properties

        internal static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register("ViewModel", typeof(MainContentViewModel), typeof(MainContentUserControl),
            new PropertyMetadata(null));

        internal MainContentViewModel ViewModel
        {
            get { return (MainContentViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        #endregion

        #region Constructors

        internal MainContentUserControl()
        {
            InitializeComponent();
        }

        internal MainContentUserControl(SqliteConnection connection, Authentication authentication, NotificationManager notificationManager)
        {
            ViewModel = new MainContentViewModel(connection, authentication);
            _notificationManager = notificationManager;
            InitializeComponent();
            DataContext = ViewModel;
            SetOnLoadFields();
        }

        #endregion

        #region Events

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                Dictionary<String, String> selectedItemDictionary = new Dictionary<String, String>();
                if (listView.SelectedItem != null)
                {
                    var itemKvp = (KeyValuePair<String, String>)listView.SelectedItem;
                    selectedItemDictionary.Add(itemKvp.Key, itemKvp.Value);
                }
                foreach (var item in selectedItemDictionary)
                {
                    ViewModel.SetSelectedItem(selectedItemDictionary.First().Key);
                    if (ViewModel.SelectedItem != null)
                    {
                        SetSelectedFieldValues();
                        SetSelectedButtonsEnabled();
                    }
                }
            }
        }

        private void xUsernameCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(xUsername.Text);
        }

        private void xPasswordCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(xPasswordHidden.Password);
        }

        private void xNew_Click(object sender, RoutedEventArgs e)
        {
            xNew.IsEnabled = false;
            xName.IsEnabled = true;
            xCancel.IsEnabled = true;
            SetTextFieldsEmpty();
            SetFieldsEnabled();
        }

        private void xEdit_Click(object sender, RoutedEventArgs e)
        {
            SetFieldsEnabled();
            SetSelectedButtonsDisabled();
        }

        private void xDeleteSave_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (clickedButton.Content.ToString() == _deleteButton)
                {
                    DeleteItem();
                }
                if (clickedButton.Content.ToString() == _saveButton)
                {
                    SaveItem();
                }
            }
        }

        private void xCancel_Click(object sender, RoutedEventArgs e)
        {
            xDeleteSave.Content = _deleteButton;
            SetFieldsDisabled();
            if (ViewModel.SelectedItem != null && !xEdit.IsEnabled)
            {
                ViewModel.SetSelectedItem(ViewModel.SelectedItem.Name);
                SetSelectedFieldValues();
                SetSelectedButtonsEnabled();
            }
            else if (ViewModel.SelectedItem != null)
            {
                xWebsites.SelectedItem = null;
                ViewModel.SelectedItem = null;
                SetButtonsDisabled();
                SetTextFieldsEmpty();
            }
            else
            {
                SetButtonsDisabled();
                SetTextFieldsEmpty();
            }
        }
        private void xPasswordReveal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            xPasswordShow.Text = xPasswordHidden.Password;
            xPasswordHidden.Visibility = Visibility.Hidden;
            xPasswordShow.Visibility = Visibility.Visible;
        }

        private void xPasswordReveal_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void xPasswordReveal_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        #endregion

        #region Methods

        private void SetOnLoadFields()
        {
            xWebsites.ItemsSource = ViewModel.WebsiteList;
            SetFieldsDisabled();
            SetButtonsDisabled();
        }

        private void SetFieldsEnabled()
        {
            xWebsite.IsEnabled = true;
            xUsername.IsEnabled = true;
            xPasswordHidden.IsEnabled = true;
            xPasswordReveal.IsEnabled = true;
            xNotes.IsEnabled = true;
            xDeleteSave.IsEnabled = true;
            xDeleteSave.Content = _saveButton;
        }

        private void DeleteItem()
        {
            var name = ViewModel.SelectedItem != null ? ViewModel.SelectedItem.Name : xName.Text;
            try
            {
                ViewModel.DeleteItem();
                var message = String.Format(Constants.ItemDeleteSuccess, name);
                _notificationManager.Show(message, NotificationType.Success);
                SetUpdatedContent();
            }
            catch
            {
                var message = String.Format(Constants.ItemDeleteFailure, name);
                _notificationManager.Show(message, NotificationType.Error);
            }
        }

        private void SaveItem()
        {
            var name = ViewModel.SelectedItem != null ? ViewModel.SelectedItem.Name : xName.Text;
            try
            {
                ViewModel.SaveItem(xName.Text, xUsername.Text, xWebsite.Text, xPasswordHidden.Password, xNotes.Text);
                var message = String.Format(Constants.ItemSaveSuccess, name);
                _notificationManager.Show(message, NotificationType.Success);
                SetUpdatedContent();
            }
            catch
            {
                var message = String.Format(Constants.ItemSaveFailure, name);
                _notificationManager.Show(message, NotificationType.Error);
            }
        }

        private void SetFieldsDisabled()
        {
            xName.IsEnabled = false;
            xWebsite.IsEnabled = false;
            xUsername.IsEnabled = false;
            xPasswordHidden.IsEnabled = false;
            xNotes.IsEnabled = false;
            xDeleteSave.Content = _deleteButton;
        }

        private void SetButtonsDisabled()
        {
            xPasswordCopy.IsEnabled = false;
            xPasswordReveal.IsEnabled = false;
            xUsernameCopy.IsEnabled = false;
            xEdit.IsEnabled = false;
            xDeleteSave.IsEnabled = false;
            xCancel.IsEnabled = false;
        }

        private void SetTextFieldsEmpty()
        {
            xName.Text = String.Empty;
            xWebsite.Text = String.Empty;
            xUsername.Text = String.Empty;
            xPasswordHidden.Password = String.Empty;
            xNotes.Text = String.Empty;
        }

        private void SetSelectedButtonsEnabled()
        {
            xNew.IsEnabled = true;
            xEdit.IsEnabled = true;
            xDeleteSave.IsEnabled = true;
            xDeleteSave.Content = _deleteButton;
            xPasswordCopy.IsEnabled = true;
            xPasswordReveal.IsEnabled = true;
            xUsernameCopy.IsEnabled = true;
            xCancel.IsEnabled = true;
        }

        private void SetSelectedButtonsDisabled()
        {
            xEdit.IsEnabled = false;
            xPasswordCopy.IsEnabled = false;
            xUsernameCopy.IsEnabled = false;
        }

        private void SetSelectedFieldValues()
        {
            if (ViewModel.SelectedItem != null)
            {
                xName.Text = ViewModel.SelectedItem.Name;
                xWebsite.Text = ViewModel.SelectedItem.Website;
                xUsername.Text = ViewModel.SelectedItem.Username;
                xPasswordHidden.Password = ViewModel.SelectedItem.Password;
                xNotes.Text = ViewModel.SelectedItem.Notes;
            }
        }

        private void HidePassword()
        {
            xPasswordShow.Text = String.Empty;
            xPasswordHidden.Visibility = Visibility.Visible;
            xPasswordShow.Visibility = Visibility.Hidden;
        }

        private void SetUpdatedContent()
        {
            if (ViewModel.Content != null)
            {
                xWebsites.ItemsSource = ViewModel.WebsiteList;
                if (ViewModel.SelectedItem != null)
                {
                    xWebsites.SelectedItem = ViewModel.WebsiteList.Where(w => w.Key == ViewModel.SelectedItem.Name).First();
                    SetSelectedFieldValues();
                    SetFieldsDisabled();
                    SetSelectedButtonsEnabled();
                }
                else
                {
                    xWebsites.SelectedItem = null;
                    SetTextFieldsEmpty();
                    SetButtonsDisabled();
                }
            }
        }

        #endregion  
    }
}
