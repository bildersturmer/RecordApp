using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordApp.Services
{
    public class UserSession: ISessionService
    {
        public string CurrentUser { get; set; }
        public string CurrentRole { get; set; }

        public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentUser);

        public event Action SessionChanged;

        public void StartSession(string username, string role)
        {
            CurrentUser = username;
            CurrentRole = role;
            SessionChanged?.Invoke(); // Notify subscribers
        }

        public void EndSession()
        {
            CurrentUser = null;
            CurrentRole = null;
            SessionChanged?.Invoke(); // Notify subscribers
        }

        public bool IsAdmin() => CurrentRole == "ALL_PRIVILEDGES";

    }
}
