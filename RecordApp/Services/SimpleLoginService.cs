using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordApp.Services
{
    public class SimpleLoginService : ILoginService
    {
        public bool ValidateCredentials(string username, string password, out string role)
        {
            if (username == "admin" && password == "admin")
            {
                role = "ALL_PRIVILEGES";
                return true;
            }
            else if (username == "user" && password == "user")
            {
                role = "RESTRICTED_PRIVILEGES";
                return true;
            }

            role = null;
            return false;
        }
    }

}
