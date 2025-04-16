using FluentValidation;
using UserManagementAPI.DAL.Dtos.Product;
using UserManagementAPI.DAL.Models;

namespace UserManagementAPI.BL.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Price).NotEmpty().WithMessage("price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.UnitsInStock).NotEmpty().WithMessage("UnitsInStock is required.")
                .GreaterThanOrEqualTo(0).WithMessage("stock must be greater than zero.");

            RuleFor(x => x.Category)
                   .NotEmpty().WithMessage("Category is required.")
                    .Must(value => Enum.TryParse<ProductCategory>(value, out _)).WithMessage("Invalid category.");
        }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.ProductName)
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProductName));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.UnitsInStock)
                .GreaterThanOrEqualTo(0).WithMessage("stock must be greater than zero.")
                .When(x => x.UnitsInStock.HasValue);

            RuleFor(x => x.Category)
                .Must(value => Enum.TryParse<ProductCategory>(value, out _))
                .WithMessage("Invalid category.")
                .When(x => !string.IsNullOrWhiteSpace(x.Category));
        }
    }
}
