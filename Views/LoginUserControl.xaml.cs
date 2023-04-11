using LockAndKey.Database;
using LockAndKey.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LockAndKey.Views
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        #region Properties

        public static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register("ViewModel", typeof(LoginUserControlViewModel), typeof(LoginUserControl),
            new PropertyMetadata(null));

        internal LoginUserControlViewModel ViewModel
        {
            get { return (LoginUserControlViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        #endregion

        internal LoginUserControl()
        {
            InitializeComponent();
        }

        internal LoginUserControl(SqliteConnection connection, Authentication authentication)
        {
            ViewModel = new LoginUserControlViewModel(connection, authentication);
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void xEnter_Click(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void xExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void xPasswordValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                CheckPassword();
            }
        }

        private void CheckPassword()
        {
            if (!String.IsNullOrEmpty(xPasswordValue.Password))
            {
                var isValid = !ViewModel.Authentication.IsLockedOut;
                if (isValid)
                {
                    isValid = ViewModel.LoginUser(xPasswordValue.Password);
                }

                if (isValid)
                {
                    var handler = LoginAuthenticationChanged;
                    if (handler != null)
                    {
                        handler(this, new LoginAuthenticationEventArgs
                        {
                            LoginAuthentication = ViewModel.Authentication
                        });
                    }
                }
            }
        }

        internal class LoginAuthenticationEventArgs : EventArgs
        {
            internal Authentication LoginAuthentication { get; set; }
        }

        internal event EventHandler<LoginAuthenticationEventArgs> LoginAuthenticationChanged;
    }
}
