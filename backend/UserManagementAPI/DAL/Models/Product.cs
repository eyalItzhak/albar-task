using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DAL.Models
{
    public enum ProductCategory
    {
        Undefined, //equal to zero
        Electronics,
        Clothing,
        Food,
        Books,
        Furniture
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [Required]
        public int UnitsInStock { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }
    }
}