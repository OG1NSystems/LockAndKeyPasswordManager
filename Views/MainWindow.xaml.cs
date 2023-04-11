using LockAndKey.ViewModels;
using Notification.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LockAndKey.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private const String _deleteButton = "DELETE";
        private const String _saveButton = "SAVE";

        private NotificationManager _notificationManager = new NotificationManager();

        #endregion

        #region Properties

        internal static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register("ViewModel", typeof(MainWindowViewModel), typeof(MainWindow),
            new PropertyMetadata(null));

        internal MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        #endregion

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            Activated -= MainWindow_Activated;
                        
            if (ViewModel.Connection != null && ViewModel.Authentication != null)
            {
                if (ViewModel.UserExists)
                {
                    var loginControl = new LoginUserControl(ViewModel.Connection, ViewModel.Authentication);
                    loginControl.LoginAuthenticationChanged += LoginControl_AuthenticationChanged;
                    ShowSelectedControl(loginControl);
                }
                else
                {
                    var createControl = new CreateUserControl(ViewModel.Connection, ViewModel.Authentication);
                    createControl.CreateAuthenticationChanged += CreateControl_CreateAuthenticationChanged;
                    ShowSelectedControl(createControl);
                }
            }
        }

        private void LoginControl_AuthenticationChanged(object? sender, LoginUserControl.LoginAuthenticationEventArgs e)
        {
            ViewModel.Authentication = e.LoginAuthentication;
            var control = sender as LoginUserControl;
            if (control != null)
            {
                var notificationType = ViewModel.Authentication.IsLoggedIn ? NotificationType.Success : NotificationType.Error;
                _notificationManager.Show(ViewModel.Authentication.LoginMessage, notificationType);
                if (ViewModel.Authentication.IsLoggedIn)
                {
                    ShowMainContent(control);
                }
            }
        }

        private void CreateControl_CreateAuthenticationChanged(object? sender, CreateUserControl.CreateAuthenticationEventArgs e)
        {
            ViewModel.Authentication = e.CreateAuthentication;
            var control = sender as CreateUserControl;
            if (control != null)
            {
                var notificationType = ViewModel.Authentication.IsLoggedIn ? NotificationType.Success : NotificationType.Error;
                _notificationManager.Show(ViewModel.Authentication.LoginMessage, notificationType);
                if (ViewModel.Authentication.IsLoggedIn)
                {
                    ShowMainContent(control);
                }
            }
        }

        private void ShowMainContent(UserControl userControl)
        {
            CloseSelectedControl(userControl);

            var mainContentControl = new MainContentUserControl(ViewModel.Connection, ViewModel.Authentication, _notificationManager);
            ShowSelectedControl(mainContentControl);
        }

        private void ShowSelectedControl(UserControl userControl)
        {
            xMainGrid.Children.Clear();
            xMainGrid.Children.Add(userControl);
        }

        private void CloseSelectedControl(UserControl userControl)
        {
            xMainGrid.Children.Remove(userControl);
        }
    }
}
