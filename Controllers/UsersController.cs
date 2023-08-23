using ImprovedPicpay.Services;
using ImprovedPicpay.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace ImprovedPicpay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _userService.GetByAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            var response = await _userService.AddAsync(model);

            if (!response.Succeeded)
                return BadRequest(response);

            return CreatedAtAction(nameof(Get), null);
        }
    }
}
