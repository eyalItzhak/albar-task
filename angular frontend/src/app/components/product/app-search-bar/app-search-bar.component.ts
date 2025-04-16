import { Component, Input  } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SearchProductByField } from '../../../types/productType';

@Component({
  selector: 'app-app-search-bar',
  imports: [FormsModule],
  templateUrl: './app-search-bar.component.html',
  styleUrl: './app-search-bar.component.scss'
})
export class AppSearchBarComponent {
  searchQuery = '';
  searchField: SearchProductByField = SearchProductByField.ProductId;
  @Input() searchByQuery!: (searchQuery: string, searchField: SearchProductByField) => void;

  
  constructor(private authService: AuthService ,private router: Router) { }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/auth']);
      },
      error: (err) => {
        console.error('Logout failed', err);
      }
    });
  }

  onSearch(): void {
    console.log('searchQuery', this.searchByQuery);
    this.searchByQuery(this.searchQuery, this.searchField);
  }
}
