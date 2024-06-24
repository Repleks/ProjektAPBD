using System.ComponentModel.DataAnnotations;

namespace ProjektAPBD.RequestModels;

public class UpdateCompanyRequestModel
{
    [Required]
    public int IdCompany { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CompanyAddress { get; set; }
    
    [Required]
    [EmailAddress]
    public string CompanyEmail { get; set; }
    
    [Required]
    [Phone]
    public string CompanyPhone { get; set; }
}