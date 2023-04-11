using LockAndKey.Database;
using LockAndKey.Helpers;
using LockAndKey.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LockAndKey.Views
{
    /// <summary>
    /// Interaction logic for CreateUserControl.xaml
    /// </summary>
    public partial class CreateUserControl : UserControl
    {
        #region Properties

        internal static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register("ViewModel", typeof(CreateUserControlViewModel), typeof(CreateUserControl),
            new PropertyMetadata(null));

        internal CreateUserControlViewModel ViewModel
        {
            get { return (CreateUserControlViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        #endregion

        internal CreateUserControl()
        {
            InitializeComponent();
        }

        internal CreateUserControl(SqliteConnection connection, Authentication authentication)
        {
            ViewModel = new CreateUserControlViewModel(connection, authentication);
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void xEnter_Click(object sender, RoutedEventArgs e)
        {
            CreateUser();
        }

        private void xExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                CreateUser();
            }
        }

        private void CreateUser()
        {
            var message = String.Empty;
            var isSuccess = true;
            if (String.IsNullOrEmpty(xNewPassword.Password) || String.IsNullOrEmpty(xConfirmPassword.Password))
            {
                message = Constants.CreateAdminEmptyField;
                isSuccess = false;
            }
            else if (xNewPassword.Password != xConfirmPassword.Password)
            {
                message = Constants.CreateAdminNotMatched;
                isSuccess = false;
            }
            else if (xNewPassword.Password.Length < 8)
            {
                message = Constants.CreateAdminInsufficientCount;
                isSuccess = false;
            }
            else
            {
                if (AuthenticationHelper.CheckSecurityStringMeetsRequirements(xNewPassword.Password))
                {
                    ViewModel.CreateUser(xNewPassword.Password);
                    message = "Master Password successfully created.";
                }
                else
                {
                    message = Constants.CreateAdminMissingCharacterTypes;
                    isSuccess = false;
                }
            }

            if (isSuccess)
            {
                var handler = CreateAuthenticationChanged;
                if (handler != null)
                {
                    handler(this, new CreateAuthenticationEventArgs
                    {
                        CreateAuthentication = ViewModel.Authentication
                    });
                }
            }
        }

        internal class CreateAuthenticationEventArgs : EventArgs
        {
            internal Authentication CreateAuthentication { get; set; }
        }

        internal event EventHandler<CreateAuthenticationEventArgs> CreateAuthenticationChanged;
    }
}
