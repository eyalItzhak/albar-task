using Microsoft.EntityFrameworkCore;
using UserManagementAPI.DAL.Context;
using UserManagementAPI.DAL.Dtos.Product;
using UserManagementAPI.DAL.Dtos.Results;
using UserManagementAPI.DAL.Models;


namespace UserManagementAPI.BL.Services
{
    public interface IProductService
    {
        Task<Result<Product>> CreateProductAsync(ProductDto product);
        Task<Result<Product>> UpdateProductAsync(int id, UpdateProductDto product);
        Task<Result> DeleteProductAsync(int id);
        Task<Result<PaginatedResult<Product>>> SearchProductsAsync(string? productName, string? category, int? id, int page, int pageSize);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Product>> CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                UnitsInStock = productDto.UnitsInStock,
                Price = productDto.Price,
                Category = productDto.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Result<Product>.Success(product); // Return success with the created product
        }

        public async Task<Result<Product>> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return Result<Product>.Failure("Product not found");
            }

            existingProduct.ProductName = productDto.ProductName ?? existingProduct.ProductName;
            existingProduct.Price = productDto.Price ?? existingProduct.Price;
            existingProduct.UnitsInStock = productDto.UnitsInStock ?? existingProduct.UnitsInStock;
            existingProduct.Category = productDto.Category ?? existingProduct.Category;

            await _context.SaveChangesAsync();
            return Result<Product>.Success(existingProduct); // Return success with updated product
        }

        public async Task<Result> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return Result.Failure("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Result.Success(); // Return success (no need to return a product)
        }

        public async Task<Result<PaginatedResult<Product>>> SearchProductsAsync(string? productName, string? category, int? id, int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            if (id.HasValue)
                query = query.Where(p => p.ProductId == id.Value);

            if (!string.IsNullOrEmpty(productName))
                query = query.Where(p => p.ProductName.Contains(productName));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));

            var totalCount = await query.CountAsync();
            var skip = (page - 1) * pageSize;

            var items = await query.Skip(skip).Take(pageSize).ToListAsync();

            var result = new PaginatedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = page
            };

            return Result<PaginatedResult<Product>>.Success(result); // Return success with paginated result
        }
    }
}
