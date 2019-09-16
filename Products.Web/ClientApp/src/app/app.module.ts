import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import {
  ProductsComponent,
  RemoveProductDielogComponent
} from './products/products.component';
import { EditProductComponent } from './edit-product/edit-product.component';
import { ProductService } from './services/product.service';
import { CustomeErrorHandler } from './services/custome-error-handler';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ProductsComponent,
    EditProductComponent,
    RemoveProductDielogComponent,
    ErrorDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatDialogModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'products', component: ProductsComponent },
      { path: 'edit-product/:id', component: EditProductComponent },
      { path: 'edit-product', component: EditProductComponent }
    ])
  ],
  entryComponents: [RemoveProductDielogComponent, ErrorDialogComponent],
  providers: [
    ProductService,
    [
      {
        provide: ErrorHandler,
        useClass: CustomeErrorHandler
      }
    ]
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
