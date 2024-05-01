using Data.Models;

namespace Business.Services.Interfaces;

public interface IApplicationUserService
{
    ApplicationUser? GetUser(string id);
    ApplicationUser? GetUserByEmail(string email);
    ApplicationUser? GetUserByUsername(string username);
    ApplicationUser? GetUserByPhoneNumber(string phoneNumber);
}
