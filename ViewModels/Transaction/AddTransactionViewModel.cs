using System.ComponentModel.DataAnnotations;

namespace ImprovedPicpay.ViewModels.Transaction;

public class AddTransactionViewModel
{
    [Required(ErrorMessage = "Please, specify the sender ID.")]
    public string From { get; set; }
    [Required(ErrorMessage = "Please, specify the receiver ID.")]
    public string To { get; set; }
    [Required(ErrorMessage = "Please, specify the amount that you want to transfer.")]
    public decimal Amount { get; set; }
}
