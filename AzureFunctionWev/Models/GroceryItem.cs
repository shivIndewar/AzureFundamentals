using System.ComponentModel.DataAnnotations;

namespace AzureFunctionWeb.Models
{
    public class GroceryItem
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
