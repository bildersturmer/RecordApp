# RecordApp
RecordApp is a WPF MVVM application for managing gas account records and payments. It provides a simple interface for recording units, calculating costs, and tracking payments.

## Features

Add and update gas account details.
Record units consumed and calculate charges.
Make payments and view outstanding balances.

Requirements

Windows 10 or later.
.NET 6 SDK installed.
Visual Studio 2022 recommended.

## Setup

Clone the repository:
git clone https://github.com/yourusername/RecordApp.git
Open RecordApp.sln in Visual Studio.
Build the solution.

## Data File
The application expects GasAccounts.xml to be located in:
%USERPROFILE%\AppData\Roaming\RecordApp
(Example: C:\Users\YourName\AppData\Roaming\RecordApp)
Create this folder if it does not exist.

## Notes

Do not commit GasAccounts.xml to the repo.
See TestingGuidance.md for optional testing strategy tips.
