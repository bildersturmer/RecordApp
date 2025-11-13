using Microsoft.Extensions.DependencyInjection;
using RecordApp.App;
using RecordApp.Models;
using RecordApp.Services;
using RecordApp.ViewModels;
using RecordApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecordApp.Infrastructure
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            var app = new App.App();
            app.InitializeComponent();


            var loginWindow = provider.GetRequiredService<LoginWindow>();
            if (loginWindow.ShowDialog() == true)
            {
                var mainWindow = provider.GetRequiredService<MainWindow>();
                var session = provider.GetRequiredService<ISessionService>();

                // Update title based on role
                mainWindow.Title = session.IsAdmin()
                    ? "Record App - Welcome, Admin"
                    : "Record App - Welcome, User";

                app.Run(mainWindow);
            }

            /*
            var loginWindow = provider.GetRequiredService<LoginWindow>();
            // LoginWindow loginWindow = provider.GetRequiredService();

            if (loginWindow.ShowDialog() == true)
            {
                // var mainWindow = provider.GetRequiredService();
                var mainWindow = provider.GetRequiredService<MainWindow>();

                if ( null == mainWindow)
                {
                    Console.WriteLine("No MainWindow");
                }

                app.Run(mainWindow);
                
            }

            */
        }

        /* TO BE REMOVED FROM 11/11/2025
        private static void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton<ILoginService, SimpleLoginService>();
            services.AddSingleton<ILoginService, SecureLoginService>();
            services.AddSingleton<ISessionService, UserSession>();
            // services.AddSingleton<ICustomerDataService, XmlCustomerDataService>();
            services.AddSingleton<LoginWindow>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
        */

        private static void ConfigureServices(IServiceCollection services)
        {
            // === Services ===
            services.AddSingleton<ILoginService, SecureLoginService>();
            services.AddSingleton<ISessionService, UserSession>();
            services.AddSingleton<IDataPersistence<GasAccount>>(provider =>
            {
                // Build the path: AppData\Roaming\RecordApp\GasAccounts.xml
                string dataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RecordApp",
                    "GasAccounts.xml"
                );

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

                // Register XmlDataPersistence with the resolved path
                return new XmlDataPersistence<GasAccount>(dataPath);
            });

            // === ViewModels ===
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>(); // Add if you create this class

            // === Views ===
            services.AddTransient<MainWindow>(); // DI will inject MainWindowViewModel
            services.AddTransient<LoginWindow>(); // DI will inject LoginViewModel
        }


    }

}
