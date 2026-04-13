
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZcommerce.Web.Models;

public class Order
{
    public int Id {get; set;}

    [Required]
    [StringLength(100)]
    public required string CustomerName {get; set;}

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public required string CustomerEmail {get; set;}

    [StringLength(10)]
    public string? CustomerPhone {get; set;}

    [Required]
    [StringLength(100)]
    public required string ShippingAddressLine1 {get; set;}

    [Required]
    [StringLength(100)]
    public required string ShippingAddressLine2 {get; set;}

    [Required]
    [StringLength(100)]
    public required string City {get; set;}

    [Required]
    [StringLength(100)]
    public required string State {get; set;}

    [Required]
    [StringLength(100)]
    public required string ZipCode {get; set;}

    [Required]
    [StringLength(100)]
    public required string Country {get; set;}

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public required decimal TotalAmmount {get; set;}

    [Required]
    [StringLength(100)]
    public required string Status {get; set;} = "Processing";

    [Required]
    [StringLength(100)]
    public DateTime CreatedAt {get; set;}
}