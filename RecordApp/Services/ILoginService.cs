
namespace RecordApp.Services
{
    public interface ILoginService
    {
        bool ValidateCredentials(string username, string password, out string role);
    }

}
