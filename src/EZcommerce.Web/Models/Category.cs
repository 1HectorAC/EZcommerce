
using System.ComponentModel.DataAnnotations;

namespace EZcommerce.Web.Models;

public class Category
{

    public int Id{get;set;}

    [Required]
    [StringLength(100)]
    public required string Name {get;set;}

    public ICollection<Product> Products {get;set;} = new List<Product>{};

}