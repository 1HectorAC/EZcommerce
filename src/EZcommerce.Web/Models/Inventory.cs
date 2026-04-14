
using System.ComponentModel.DataAnnotations;

namespace EZcommerce.Web.Models;

public class Inventory
{
    public int Id {get; set;}

    [Required]
    public int ProductId {get; set;}

    public Product? Product {get; set;}

    [Required]
    [Range(0,999999999)]
    public int Quantity {get; set;} = 0;

    

}