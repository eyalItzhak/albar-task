namespace UserManagementAPI.DAL.Dtos.Product
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public int UnitsInStock { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    public class ProductSearchQueryDto
    {
        public int? Id { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class UpdateProductDto
    {
        public string? ProductName { get; set; }
        public int? UnitsInStock { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
    }

}
