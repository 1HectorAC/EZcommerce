
using System.ComponentModel.DataAnnotations;

namespace EZcommerce.Web.Models.ViewModels;

public class ProductDetailsViewModel
{
    public int Id {get; set;}

    [Required]
    public required string Name {get; set;}

    [Required]
    public string Description {get; set;} = "";

    [Required]
    public decimal Price {get; set;}

    [MaxLength(256)]
    public string? ImageUrl {get; set;}

    [Required]
    public required string Category {get; set;}

    // Maybe add inventory for stock check

    
}