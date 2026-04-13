
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZcommerce.Web.Models;

public class Payment {
    public int Id {get; set;}

    [Required]
    public int OrderId {get; set;}

    public Order? Order {get; set;}

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount {get; set;}

    [Required]
    [StringLength(100)]
    public required string Method {get; set;}

    //Consider making StatusType Model
    [Required]
    [StringLength(100)]
    public required string Status {get; set;} = "Processing";

    [Required]
    [StringLength(200)]
    public required string TransactionReference {get; set;}
}