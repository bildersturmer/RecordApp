using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecordApp.ViewModels;
using RecordApp.Models;
using RecordApp.App;

namespace RecordApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<GasAccount> accounts = new List<GasAccount>();
        // private ListBox RecordListBox;


        // private MainWindowViewModel viewModel;


        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            this.Closing += MainWindow_Closing;

            // accounts.Add( new GasAccount { intAccRefNo = 1001, strName = "John Smith", strAddress = "123 Main Street" } );
            // accounts.Add(new GasAccount { intAccRefNo = 1001, strName = "John Smith", strAddress = "123 Main Street" });
            // accounts.Add(new GasAccount { intAccRefNo = 1002, strName = "Yasmina Muntean", strAddress = "456 Oak Avenue" });
            // accounts.Add(new GasAccount { intAccRefNo = 1003, strName = "Alice Brown", strAddress = "789 Pine Road" });

            // Tell ListBox to display the accounts list
            // RecordListBox.ItemsSource = accounts;

            // Tell it what text to display (the strName field)
            // RecordListBox.DisplayMemberPath = "StrName";


            // Bind the ListBox to the ViewModel's Accounts
            // 06/11/2025 RecordListBox.ItemsSource = viewModel.Accounts;
            // 06/11/2025 RecordListBox.DisplayMemberPath = "Name";




        }

        /*
        private void RecordListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (RecordListBox.SelectedItem is GasAccount selected)
            {
                txtAccRefNo.Text = selected.AccRefNo.ToString();
                txtName.Text = selected.Name;
                txtAddress.Text = selected.Address;
                txtUnits.Text = selected.Units.ToString();
                txtBalance.Text = selected.Balance.ToString();
                txtUnitCost.Text = GasAccount.UnitCost.ToString();
                
            }
        }
        */


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // GasAccount.setUnitCost(10.00);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Close any open dialogs here if needed
            Application.Current.Shutdown(); // Properly shuts down the application
        }


        /* 06/11/2025
        private void txtRecordUnits_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null)
                {
                    string input = txtRecordUnits.Text;
                    viewModel.ProcessRecordUnitsInput(input);
                    txtRecordUnits.Text = string.Empty; // Clear the input field
                }
            }
        }
        */

    }
}