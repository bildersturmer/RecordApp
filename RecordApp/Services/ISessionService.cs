using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordApp.Services
{
    public interface ISessionService
    {
        string CurrentUser { get; set; }

        string CurrentRole { get; set; }

        bool IsAuthenticated { get; }

        bool IsAdmin();

        void StartSession(string username, string role);

        void EndSession();

        event Action SessionChanged;
    }
}
