using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.PaymentContractRequestModels;

public class PostPaymentContractRequestModel
{
    [Required]
    public int PaymentId { get; set; }
    
    [Required]
    public int ContractId { get; set; }
    
    [Required]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    public double Amount { get; set; }
    
    [Required]
    public string PaymentInformation { get; set; }
}