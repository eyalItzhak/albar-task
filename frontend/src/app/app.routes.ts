import { Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { RedirectIfLoggedInGuard , requireAuthGuard } from './guards/auth.guard';
import { ProductComponent } from './components/product/prodcut/product.component';


export const routes: Routes = [
    { path: '', redirectTo: 'auth', pathMatch: 'full' },
    { path: 'auth', component: AuthComponent , canActivate: [RedirectIfLoggedInGuard] },
    { path: 'product', component: ProductComponent, canActivate: [requireAuthGuard] , }
];
