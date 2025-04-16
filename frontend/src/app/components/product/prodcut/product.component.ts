import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product, ProductCategory, SearchProductByField } from '../../../types/productType';
import { SearchProductFilter } from '../../../types/filters';
import { AppSearchBarComponent } from '../app-search-bar/app-search-bar.component';
import { SnackBarService } from '../../../services/snack-bar.service';
import { ProductFormComponent } from '../product-form/product-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ErrorStandartize } from '../../../utils/errorUtils';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    FormsModule,
    AppSearchBarComponent,
    ProductFormComponent,
    MatFormFieldModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  styleUrls: ['./product.component.scss'],
  standalone: true
})
export class ProductComponent implements OnInit {

  categories = Object.values(ProductCategory).filter(
    value => isNaN(Number(value))
  );
  displayedColumns: string[] = ['productId', 'productName', 'price', 'unitsInStock', 'category', 'actions'];
  products: Product[] = [];
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 20];
  isLoading = false;
  editingProductId: number | null = null;
  editedProduct: Partial<Product> = {};
  filters: SearchProductFilter = {};
  constructor(private productService: ProductService, private snackBarService: SnackBarService) { }

  ngOnInit(): void {
    this.fetchProducts();
  }

  fetchProducts(): void {
    this.isLoading = true;
    const page = this.pageIndex + 1;

    this.productService.getProducts(page, this.pageSize, this.filters).subscribe({
      next: (res) => {
        if (res) {
          this.products = res.items ?? [];
          this.totalItems = res.totalCount;
        } else {
          this.snackBarService.show("not found any products");
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
      },
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.fetchProducts();
  }

  editProduct(product: Product): void {
    this.editingProductId = product.productId;
    this.editedProduct = { ...product };
  }

  cancelEdit(): void {
    this.editingProductId = null;
    this.editedProduct = {};
  }

  deleteProduct(productId: number): void {
    if (!confirm("Are you sure you want to delete this product?")) return;

    this.productService.deleteProduct(productId).subscribe({
      next: () => {
        this.snackBarService.show("Success - Product deleted successfully");
        this.fetchProducts();
      },
      error: (err) => {
        this.snackBarService.show("Error", ErrorStandartize(err) || "Failed to delete product");
        console.error('Error deleting product', err);
      }
    });
  }

  saveEdit(productId: number): void {
    const index = this.products.findIndex(p => p.productId === productId);
    if (index !== -1) {
      this.products[index] = { ...this.products[index], ...this.editedProduct };
    }
    this.productService.updateProduct(productId, this.editedProduct).subscribe({
      next: (res) => {
        this.snackBarService.show("Success - Product was update successfully");
        this.cancelEdit();
        this.fetchProducts();
      },
      error: (err) => {
        this.snackBarService.show(`error` + ErrorStandartize(err) || "Failed to update product");
        console.error('Error updating product', err);
      },
    });
  }

  searchByQuery(searchQuery: string, searchField: SearchProductByField): void {
    const query = searchQuery.trim();

    if (!query) {
      this.filters = {};
      this.fetchProducts();
      return;
    }

    this.filters = {
      [searchField]: query
    };
    this.pageIndex = 0; // Reset to the first page
    this.fetchProducts();
  }
}
