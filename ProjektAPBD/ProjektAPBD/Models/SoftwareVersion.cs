using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAPBD.Models;

[Table("SoftwareVersions")]
public class SoftwareVersion
{
    [Key]
    [Required]
    public int IdSoftwareVersion { get; set; }
    
    [Required]
    [ForeignKey("Software")]
    public int IdSoftware { get; set; }
    
    public Software Software { get; set; }
    
    [Required]
    public string Version { get; set; }
}