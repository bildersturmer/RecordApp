using RecordApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RecordApp.Models;
using RecordApp.Infrastructure;

namespace RecordApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _isAdmin;
        private bool _isRecordUnitsReadOnly = true;
        private bool _isDepositReadOnly = true;
        private bool _isUnitPriceReadOnly = true;

        private readonly IDataPersistence<GasAccount> _persistence;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        public bool IsRecordUnitsReadOnly
        {
            get => _isRecordUnitsReadOnly;
            set
            {
                if (_isRecordUnitsReadOnly != value)
                {
                    _isRecordUnitsReadOnly = value;
                    OnPropertyChanged(nameof(IsRecordUnitsReadOnly));
                }
            }
        }

        public bool IsDepositReadOnly
        {
            get => _isDepositReadOnly;
            set
            {
                if (_isDepositReadOnly != value)
                {
                    _isDepositReadOnly = value;
                    OnPropertyChanged(nameof(IsDepositReadOnly));
                }
            }
        }

        public bool IsUnitPriceReadOnly
        {
            get => _isUnitPriceReadOnly;
            set
            {
                if (_isUnitPriceReadOnly != value)
                {
                    _isUnitPriceReadOnly = value;
                    OnPropertyChanged(nameof(IsUnitPriceReadOnly));
                }
            }
        }

        public double UnitCost => GasAccount.UnitCost;
        public ObservableCollection<GasAccount> Accounts { get; set; }

        private GasAccount _selectedAccount;
        public GasAccount SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                Debug.WriteLine($"SelectedAccount changed: {(SelectedAccount != null)}");
                OnPropertyChanged(nameof(SelectedAccount));
                (DeleteCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand RecordUnitsCommand { get; }
        public ICommand DepositCommand { get; }
        public ICommand UnitPriceCommand { get; }
        public ICommand LoadCustomersCommand { get; }
        public ICommand SaveCustomersCommand { get; }
        public ICommand NewCustomerCommand { get; }
        public ICommand ConfirmNewCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }

        public MainWindowViewModel(ISessionService session, IDataPersistence<GasAccount> persistence)
        {
            IsRecordUnitsReadOnly = false;
            IsDepositReadOnly = false;
            IsUnitPriceReadOnly = false;

            _persistence = persistence;

            // Subscribe to session changes
            session.SessionChanged += () =>
            {
                IsAdmin = session.IsAdmin();
                RefreshAdminCommands();
                Debug.WriteLine($"SessionChanged fired. IsAdmin={IsAdmin}");
            };

            _isAdmin = session.IsAdmin();
            RefreshAdminCommands();

            // Initialize Accounts collection and load from XML
            Accounts = new ObservableCollection<GasAccount>();
            LoadCustomers(); // Uses _persistence.ReadDataRecords()

            /*
            List<GasAccount> hardCodedAccounts = new List<GasAccount>();
            hardCodedAccounts.Add(new GasAccount { AccRefNo = 1001, Name = "John Smith", Address = "123 Main Street" });
            hardCodedAccounts.Add(new GasAccount { AccRefNo = 1002, Name = "Yasmina Muntean", Address = "456 Oak Avenue" });
            hardCodedAccounts.Add(new GasAccount { AccRefNo = 1003, Name = "Alice Brown", Address = "789 Pine Road" });
            Accounts = new ObservableCollection<GasAccount>(hardCodedAccounts);
            */

            RecordUnitsCommand = new RelayCommand(
                execute: _ =>
                {
                    if (SelectedAccount == null)
                    {
                        MessageBox.Show("Please select a customer name first.");
                        RecordUnitsInput = string.Empty;
                        return;
                    }
                    if (!int.TryParse(RecordUnitsInput, out int unitsUsed) || unitsUsed <= 0)
                    {
                        MessageBox.Show("Please enter a valid positive whole number.");
                        RecordUnitsInput = string.Empty;
                        return;
                    }
                    string result = SelectedAccount.RecordUnits(unitsUsed);
                    OnPropertyChanged(nameof(SelectedAccount));
                    MessageBox.Show(result);
                    RecordUnitsInput = string.Empty;
                },
                canExecute: _ => true
            );

            DepositCommand = new RelayCommand(
                execute: _ =>
                {
                    if (SelectedAccount == null)
                    {
                        MessageBox.Show("Please select a Customer Name first.");
                        RecordUnitsInput = string.Empty;
                        return;
                    }
                    if (!double.TryParse(DepositInput, out double depositAmount) || depositAmount <= 0)
                    {
                        MessageBox.Show("Please enter a valid positive whole number.");
                        DepositInput = string.Empty;
                        return;
                    }
                    SelectedAccount.Deposit(depositAmount);
                    MessageBox.Show("Deposit successful");
                    OnPropertyChanged(nameof(SelectedAccount));
                    DepositInput = string.Empty;
                },
                canExecute: _ => true
            );

            UnitPriceCommand = new RelayCommand(
                execute: _ =>
                {
                    if (!double.TryParse(UnitPriceInput, out double unitPriceAmount) || unitPriceAmount <= 0)
                    {
                        MessageBox.Show("Please enter a valid positive currency value.");
                        UnitPriceInput = string.Empty;
                        return;
                    }
                    GasAccount.UnitCost = unitPriceAmount;
                    MessageBox.Show("Update to Unit Price was successful.");
                    OnPropertyChanged(nameof(UnitCost));
                    UnitPriceInput = string.Empty;
                },
                canExecute: _ => true
            );

            LoadCustomersCommand = new RelayCommand(_ => LoadCustomers());
            SaveCustomersCommand = new RelayCommand(_ => SaveCustomers());

            DeleteCustomerCommand = new RelayCommand(
                _ => DeleteSelectedCustomer(),
                _ =>
                {
                    Debug.WriteLine($"CanExecute check: IsAdmin={IsAdmin}, SelectedAccount={(SelectedAccount != null)}");
                    return IsAdmin && SelectedAccount != null;
                }
            );
        }

        private void RefreshAdminCommands()
        {
            (DeleteCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (NewCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ConfirmNewCustomerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (UnitPriceCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void LoadCustomers()
        {
            Accounts.Clear();
            var records = _persistence.ReadDataRecords();
            if (records != null)
            {
                foreach (var customer in records)
                    Accounts.Add(customer);
            }
        }

        private void SaveCustomers()
        {
            _persistence.ClearAllStoredDataRecords();
            _persistence.WriteDataRecords(Accounts.ToArray());
            MessageBox.Show("Customer data saved successfully.");
        }

        private void DeleteSelectedCustomer()
        {
            if (SelectedAccount != null)
            {
                Accounts.Remove(SelectedAccount);
                MessageBox.Show("Customer deleted.");
            }
        }

        private string _recordUnitsInput;
        public string RecordUnitsInput
        {
            get => _recordUnitsInput;
            set
            {
                _recordUnitsInput = value;
                OnPropertyChanged(nameof(RecordUnitsInput));
            }
        }

        private string _depositInput;
        public string DepositInput
        {
            get => _depositInput;
            set
            {
                _depositInput = value;
                OnPropertyChanged(nameof(DepositInput));
            }
        }

        private string _unitPriceInput;
        public string UnitPriceInput
        {
            get => _unitPriceInput;
            set
            {
                _unitPriceInput = value;
                OnPropertyChanged(nameof(UnitPriceInput));
            }
        }

        private string _newName;
        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(nameof(NewName)); }
        }

        private string _newAddress;
        public string NewAddress
        {
            get => _newAddress;
            set { _newAddress = value; OnPropertyChanged(nameof(NewAddress)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}