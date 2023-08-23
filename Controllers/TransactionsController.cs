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

        [HttpPost]
        public async Task<IActionResult> Add(AddTransactionViewModel model)
        {
            var response = await _transactionService.CreateAsync(model);

            return Ok(response);
        }
    }
}
