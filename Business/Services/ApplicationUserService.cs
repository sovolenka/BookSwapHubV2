using Business.Services.Interfaces;
using Data;
using Data.Models;

namespace Business.Services;

public class ApplicationUserService : IApplicationUserService
{
    readonly PostgresContext _context;

    public ApplicationUserService(PostgresContext context)
    {
        _context = context;
    }

    public ApplicationUser? GetUser(string id)
    {
        return _context.Set<ApplicationUser>().Find(id);
    }

    public ApplicationUser? GetUserByEmail(string email)
    {
        return _context.Set<ApplicationUser>().FirstOrDefault(u => u.Email == email);
    }

    public ApplicationUser? GetUserByUsername(string username)
    {
        return _context.Set<ApplicationUser>().FirstOrDefault(u => u.UserName == username);
    }

    public ApplicationUser? GetUserByPhoneNumber(string phoneNumber)
    {
        return _context.Set<ApplicationUser>().FirstOrDefault(u => u.PhoneNumber == phoneNumber);
    }
}
