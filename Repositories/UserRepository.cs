using ImprovedPicpay.Data;
using ImprovedPicpay.Helpers;
using ImprovedPicpay.Mappers;
using ImprovedPicpay.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ServiceResponse<bool>> AddAsync(User user)
        {
            try
            {
                if (IsEmailTaken(user.Email))
                    return new ServiceResponse<bool>
                    {
                        Succeeded = false,
                        Message = "The user email is not available."
                    };

                if (IsDocumentTaken(user.Document))
                    return new ServiceResponse<bool>
                    {
                        Succeeded = false,
                        Message = "The user document is already in use."
                    };

                user.Id = Guid.NewGuid().ToString();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Succeeded = true,
                    Message = "User created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message, ex.Message, "-- Error while creating a new user. --", DateTime.Now);

                return new ServiceResponse<bool>
                {
                    Succeeded = false,
                    Message = "An error occorred while creating a user"
                };
            }
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(User user)
        {
            try
            {
                if (IsDocumentTaken(user.Document))
                    return new ServiceResponse<bool>
                    {
                        Succeeded = false,
                        Message = "The user document is already in use."
                    };

                User existingUser = await GetByAsync(user.Id);

                if (existingUser == null)
                    return new ServiceResponse<bool>
                    {
                        Succeeded = false,
                        Message = "User does not exist."
                    };

                if (!existingUser.Email.Equals(user.Email) && IsEmailTaken(user.Email))
                    return new ServiceResponse<bool>
                    {
                        Succeeded = false,
                        Message = "The user email is not available."
                    };

                existingUser.MapFromUserToUser(user);

                _context.Entry(existingUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Succeeded = true,
                    Message = "User created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message, ex.Message, "-- Error while creating a new user. --", DateTime.Now);

                return new ServiceResponse<bool>
                {
                    Succeeded = false,
                    Message = "An error occorred while creating a user"
                };
            }
        }

        public async Task<User> GetByAsync(string id) => await _context.Users.Where(us => us.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();

        public bool IsEmailTaken(string email) => _context.Users.Any(us => us.Email.Equals(email));
        public bool IsDocumentTaken(string document) => _context.Users.Any(us => us.Id.Equals(document));
    }
}
