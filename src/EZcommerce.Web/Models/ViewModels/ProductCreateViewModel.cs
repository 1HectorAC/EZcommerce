

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZcommerce.Web.Models;

public class ProductCreateViewModel
{

    public int Id {get; set;}

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(256)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public Decimal Price { get; set; }

    [MaxLength(256)]
    public string? ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [Range(0,999999999)]
    public int InventoryQuantity { get; set; }
}