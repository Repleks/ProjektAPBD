using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Companies")]
public class Company
{
    [Key]
    [Required]
    [Column("IdCompany")]
    public int CompanyId { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Name")]
    public string CompanyName { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("Address")]
    public string CompanyAddress { get; set; }
    
    [Required]
    [EmailAddress]
    [Column("Email")]
    public string CompanyEmail { get; set; }
    
    [Required]
    [Phone]
    [Column("Phone")]
    public string CompanyPhone { get; set; }
    
    [Required]
    [MaxLength(9)]
    [Column("KRS")]
    public string CompanyKRS { get; set; }
    
    public ICollection<Customer> Customers { get; set; }
}