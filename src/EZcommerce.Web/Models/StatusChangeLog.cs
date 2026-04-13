
using System.ComponentModel.DataAnnotations;

namespace EZcommerce.Web.Models;

public class StatusChangeLog
{
    public int Id {get; set;}

    [Required]
    [StringLength(100)]
    public required  string PreviousStatus {get; set;}

    [Required]
    [StringLength(100)]
    public required string NewStatus {get; set;}

    [Required]
    [StringLength(100)]
    public string? User {get; set;}

    [Required]
    public bool ByAutomaticSystem {get; set;}

    [Required]
    [StringLength(100)]
    public DateTime CreatedAt {get; set;}
}