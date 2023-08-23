using ImprovedPicpay.Helpers;
using ImprovedPicpay.Mappers;
using ImprovedPicpay.Repositories;
using ImprovedPicpay.ViewModels.Users;

namespace ImprovedPicpay.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetUsersViewModel>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return UserMapper.MapToViewModel(users);
        }

        public async Task<GetUsersViewModel> GetByAsync(string id)
        {
            var user = await _userRepository.GetByAsync(id);
            return UserMapper.MapToViewModel(user);
        }

        public async Task<ServiceResponse<bool>> AddAsync(AddUserViewModel viewModel)
        {
            return await _userRepository.AddAsync(UserMapper.MapToUser(viewModel));
        }
    }
}
