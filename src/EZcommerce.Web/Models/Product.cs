
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace EZcommerce.Web.Models;

public class Product
{
    public int Id {get; set;}

    [Required]
    [MaxLength(100)]
    public required string Name {get; set;}

    [MaxLength(256)]
    public string? Description {get; set;}

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public Decimal Price {get; set;}


    [MaxLength(256)]
    public string? ImageUrl {get; set;}

    [Required]
    public int CategoryId {get; set;}

    public Category? Category {get;set;}

    [Required]
    public DateTime Created_at {get; set;}

    public Inventory? Inventory {get; set;}

}