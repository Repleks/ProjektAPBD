using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("Customers")]
public class Customer
{
    [Required]
    [Key]
    [Column("IdCustomer")]
    public int CustomerId { get; set; }
    
    [ForeignKey("Person")]
    public int? PersonId { get; set; }
    
    public Person? Person { get; set; }
    
    [ForeignKey("Company")]
    public int? CompanyId { get; set; }
    
    public Company? Company { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
    
    public ICollection<Subscription> Subscriptions { get; set; }
}