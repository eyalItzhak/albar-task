import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BaseProduct, Pagination, Product } from '../types/productType';
import { SearchProductFilter } from '../types/filters';
import { delay } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}
  private baseUrl = environment.baseUrl;
  getProducts(
    page: number,
    size: number,
    filters?: SearchProductFilter
  ): Observable<Pagination<Product>> {
    const params: string[] = [];

    params.push(`page=${page}`, `pageSize=${size}`);
  
    if (filters?.productName) {
      params.push(`productName=${encodeURIComponent(filters.productName)}`);
    }
  
    if (filters?.category) {
      params.push(`category=${encodeURIComponent(filters.category)}`);
    }
  
    if (filters?.id !== undefined) {
      params.push(`id=${filters.id}`);
    }
  
    const queryString = params.join('&');
  
    return this.http.get<Pagination<Product>>(
      `${this.baseUrl}/api/products/search?${queryString}`,
      { withCredentials: true }
    ) .pipe(delay(2000)); //delay for 2 seconds to simulate a network delay
  }

  updateProduct(id: number, product: Partial<Product>): Observable<Product> {
    return this.http.put<Product>(`${this.baseUrl}/api/products/${id}`, product, { withCredentials: true });
  }

  addProduct(product: BaseProduct): Observable<Product> {
    return this.http.post<Product>(`${this.baseUrl}/api/products`, product, {
      withCredentials: true
    });
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/products/${id}`, { withCredentials: true });
  }
}