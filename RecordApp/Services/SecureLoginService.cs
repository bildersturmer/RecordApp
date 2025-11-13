using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCrypt.Net;

namespace RecordApp.Services
{
    public class SecureLoginService : ILoginService
    {
        private readonly Dictionary<string, string> _users = new()
        {
            { "admin", "$2b$12$I.ENJggthl4KQ2mTFRAfMu0ZyPpBh7hm3n2snm7okMKQMkOfJdpCO" },
            { "user","$2b$12$3x92XQ5GzkPh5sM.XSjCvuhHszngNWHE7InIoYebre8Y1Vs2cCMRO" }
        };


        public bool ValidateCredentials(string username, string password, out string role)
        {
            role = null;
            if (!_users.ContainsKey(username))
            {
                return false;
            }

            string storedHash = _users[username];
            // Verify passsword using BCrypt
            bool isValid = BC.Verify(password, storedHash);
            if (isValid)
            {
                role = username == "admin" ? "ALL_PRIVILEDGES" : "RESTRICTED_PRIVILEDGES";
                return true;
            }

            return false;

        }

    }

}
