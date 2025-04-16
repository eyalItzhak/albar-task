import { Component, Input } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { BaseProduct, ProductCategory } from '../../../types/productType';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { MatSelectModule } from '@angular/material/select';
import { ErrorStandartize } from '../../../utils/errorUtils';
import { SnackBarService } from '../../../services/snack-bar.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss'],
  imports: [
    CommonModule,
    MatSelectModule,        
    MatFormFieldModule,    
    MatInputModule,         
    FormsModule,            
    MatProgressBarModule
  ],
})
export class ProductFormComponent {
  newProduct: BaseProduct = {
    productName: '',
    price: 0,
    unitsInStock: 0,
    category: ''
  };
  
  productCategories = Object.values(ProductCategory).filter(value => isNaN(Number(value))); 
  @Input() fetchProducts!: () => void;
  isSaving = false;
  successMessage = '';
  errorMessage = '';
  showForm = false;

  constructor(private productService: ProductService ,private snackBarService: SnackBarService) {}
 

  submitForm() {
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.productService.addProduct(this.newProduct).subscribe({
      next: (product) => {
        this.successMessage = `Product "${product.productName}" added successfully (ID: ${product.productId})`;
        this.newProduct = { productName: '', price: 0, unitsInStock: 0, category: '' };
        this.isSaving = false;
        this.fetchProducts()
      },
      error: (err) => {
          this.snackBarService.show(`error` + ErrorStandartize(err) || "Failed to update product");
        
        this.isSaving = false;
      }
    });
  }
}
