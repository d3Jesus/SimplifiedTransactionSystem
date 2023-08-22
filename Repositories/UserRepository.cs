using ImprovedPicpay.Data;
using ImprovedPicpay.Models;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
