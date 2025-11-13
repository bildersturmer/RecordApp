using System;
using System.Windows;
using System.Windows.Controls;
using RecordApp.ViewModels;

namespace RecordApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Handle PasswordBox since WPF doesn't allow direct binding
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        // React to login success and close the dialog
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (DataContext is LoginViewModel vm)
            {
                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && vm.IsLoginSuccessful)
                    {
                        DialogResult = true; // Makes ShowDialog() return true
                        Close();
                    }
                };
            }
        }
    }
}