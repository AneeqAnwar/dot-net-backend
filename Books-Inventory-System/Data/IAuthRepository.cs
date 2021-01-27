using Books_Inventory_System.Models;

namespace Books_Inventory_System.Data
{
    public interface IAuthRepository
    {
        ServiceResponse<int> Register(User user, string password);
        ServiceResponse<string> Login(string username, string password);
        bool UserExists(string username);
    }
}
