using ImprovedPicpay.Services;
using ImprovedPicpay.ViewModels.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace ImprovedPicpay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _transactionService.GetAllAsync());
        }

        [HttpGet("{senderId}")]
        public async Task<IActionResult> Get(string senderId)
        {
            return Ok(await _transactionService.GetByAsync(senderId));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTransactionViewModel model)
        {
            var response = await _transactionService.CreateAsync(model);

            return Ok(response);
        }
    }
}
