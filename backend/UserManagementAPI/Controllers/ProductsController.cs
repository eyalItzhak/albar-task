using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.BL.Services;
using UserManagementAPI.DAL.Dtos.Product;


namespace UserManagementAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors); // Return 400 BadRequest with errors
            }

            var createdProduct = result.Value;
            var searchedProduct = await _productService.SearchProductsAsync(
                productName: createdProduct.ProductName,
                category: createdProduct.Category,
                id: createdProduct.ProductId,
                page: 1,
                pageSize: 1
            );

            if (!searchedProduct.IsSuccess || !searchedProduct.Value.Items.Any())
            {
                return NotFound(); // Return 404 if the product wasn't found
            }

            return StatusCode(201, searchedProduct.Value.Items.First()); // Return 201 with the created product
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto UpdateProductDto)
        {
            var result = await _productService.UpdateProductAsync(id, UpdateProductDto);

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors); // Return 404 if the product wasn't found
            }

            return Ok(result.Value); // Return 200 with the updated product
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors); // Return 404 if there was an issue
            }

            return NoContent(); // Return 204 No Content if deletion is successful
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchQueryDto query)
        {
            var result = await _productService.SearchProductsAsync(query.ProductName, query.Category, query.Id, query.Page, query.PageSize);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors); // Return 400 BadRequest with errors
            }

            if (!result.Value.Items.Any())
            {
                return NoContent();
            }

            return Ok(result.Value); // Return 200 with the paginated result
        }


    }
}
