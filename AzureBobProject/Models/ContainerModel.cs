using System.ComponentModel.DataAnnotations;

namespace AzureBobProject.Models
{
    public class ContainerModel
    {
        [Required]
        public string Name { get; set; }
    }
}
