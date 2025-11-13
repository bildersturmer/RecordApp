
using System;
using System.ComponentModel;

namespace RecordApp.Models
{
    public class GasAccount : INotifyPropertyChanged
    {
        // Static property
        public static double UnitCost { get; set; } = 0.2;


        // Instance fields
        private int _accRefNo;
        private string _name;
        private string _address;
        private double _balance;
        private double _units;

        // Properties with change notification
        public int AccRefNo
        {
            get => _accRefNo;
            set
            {
                if (_accRefNo != value)
                {
                    _accRefNo = value;
                    OnPropertyChanged(nameof(AccRefNo));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public double Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        public double Units
        {
            get => _units;
            set
            {
                if (_units != value)
                {
                    _units = value;
                    OnPropertyChanged(nameof(Units));
                }
            }
        }

        // Constructors
        public GasAccount()
        {
            AccRefNo = -999;
            Name = "No Name";
            Address = "No Address";
            Units = -9.99;
            Balance = -9.99;
        }

        public GasAccount(int accRefNo, string name, string address, double units)
        {
            AccRefNo = accRefNo;
            Name = name;
            Address = address;
            Units = units;
            Balance = units * UnitCost;
        }

        public GasAccount(int accRefNo, string name, string address)
        {
            AccRefNo = accRefNo;
            Name = name;
            Address = address;
            Units = 0;
            Balance = 0;
        }

        // Business logic
        public void Deposit(double amount)
        {
            Balance -= amount;
        }

        public string RecordUnits(double unitsUsed)
        {
            if (unitsUsed > 0)
            {
                double cost = unitsUsed * UnitCost;
                Balance += cost;
                Units += unitsUsed;
                return "Transaction Successful";
            }
            return "Transaction Unsuccessful";
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
