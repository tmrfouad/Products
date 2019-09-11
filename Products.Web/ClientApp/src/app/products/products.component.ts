import { Component, OnInit } from '@angular/core';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  products: Product[];

  constructor(private prodService: ProductService, private router: Router) {}

  ngOnInit() {
    this.prodService.getAll().subscribe((products: Product[]) => {
      this.products = products;
    });
  }

  onRemoveBtnClicked(productId: number) {
    this.prodService.delete(productId).subscribe(() => {
      this.products = this.products.filter(p => p.id !== productId);
    });
  }

  onAddBtnClicked() {
    this.router.navigate(['/edit-product']);
  }
}
