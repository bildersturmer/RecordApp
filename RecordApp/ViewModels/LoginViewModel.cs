/*
 * using RecordApp.Infrastructure;
using RecordApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using RecordApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RecordApp.Infrastructure;



namespace RecordApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ILoginService _loginService;

        private string _username;
        private string _password;
        private bool _isLoginSuccessful;
        private string _userRole;

        public event PropertyChangedEventHandler PropertyChanged;

        // === Properties bound to UI ===
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public bool IsLoginSuccessful
        {
            get => _isLoginSuccessful;
            private set { _isLoginSuccessful = value; OnPropertyChanged(); }
        }

        public string UserRole
        {
            get => _userRole;
            private set { _userRole = value; OnPropertyChanged(); }
        }

        // === Command for Login ===
        public ICommand LoginCommand { get; }

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            bool success = _loginService.ValidateCredentials(Username, Password, out string role);
            UserRole = role;
            IsLoginSuccessful = success;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
*/

using RecordApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RecordApp.Infrastructure;
using System.Diagnostics;

namespace RecordApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ILoginService _loginService;
        private readonly ISessionService _sessionService;

        private string _username;
        private string _password;
        private bool _isLoginSuccessful;
        private string _userRole;

        public event PropertyChangedEventHandler PropertyChanged;

        // === Properties bound to UI ===
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public bool IsLoginSuccessful
        {
            get => _isLoginSuccessful;
            private set { _isLoginSuccessful = value; OnPropertyChanged(); }
        }

        public string UserRole
        {
            get => _userRole;
            private set { _userRole = value; OnPropertyChanged(); }
        }

        // === Command for Login ===
        public ICommand LoginCommand { get; }

        public LoginViewModel(ILoginService loginService, ISessionService sessionService)
        {
            _loginService = loginService;
            _sessionService = sessionService;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            bool success = _loginService.ValidateCredentials(Username, Password, out string role);
            UserRole = role;
            IsLoginSuccessful = success;

            if (success)
            {
                _sessionService.StartSession(Username, role);
                Debug.WriteLine($"Login successful: Username={Username}, Role={role}");
            }
            else
            {
                Debug.WriteLine($"Login failed for Username={Username}");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

