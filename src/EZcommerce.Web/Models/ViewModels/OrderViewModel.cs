
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZcommerce.Web.Models.ViewModels;

public class OrderViewModel
{
    public int Id {get; set;}

    [StringLength(100)]
    public string? CustomerName {get; set;}

    [EmailAddress]
    [StringLength(254)]
    public string? CustomerEmail {get; set;}

    [StringLength(10)]
    public string? CustomerPhone {get; set;}


    [StringLength(100)]
    public  string? ShippingAddressLine1 {get; set;}


    [StringLength(100)]
    public string? ShippingAddressLine2 {get; set;}


    [StringLength(100)]
    public string? City {get; set;}

    [StringLength(100)]
    public string? State {get; set;}

    [StringLength(100)]
    public string? ZipCode {get; set;}

    [StringLength(100)]
    public string? Country {get; set;}

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmmount {get; set;}

    // consider adding Status Model
    [Required]
    [StringLength(100)]
    public required string Status {get; set;} = "Processing";

}