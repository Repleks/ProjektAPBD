using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels.ContractRequestModels;

public class PostContractRequestModel
{
    [Required]
    public DateTime ContractDateFrom { get; set; }
    
    [Required]
    public DateTime ContractDateTo { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public string UpdateInformation { get; set; }
    
    public int AdditionalSupportInYears { get; set; }
    
    [Required]
    public int Software { get; set; }
    
    [Required]
    public string SoftwareVersion { get; set; }
    
    [Required]
    public int IdCustomer { get; set; }
    
    
    
}