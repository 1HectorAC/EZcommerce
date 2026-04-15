
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZcommerce.Web.Models.Session;

public class CartItem
{
    public int ProductId {get; set;}

    public required string Name {get; set;}

    public int Quantity {get; set;} = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceSnapshot {get; set;}

}