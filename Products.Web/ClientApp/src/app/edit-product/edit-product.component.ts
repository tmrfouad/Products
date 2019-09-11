import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Product } from '../models/product';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html'
})
export class EditProductComponent implements OnInit {
  productForm = new FormGroup({
    id: new FormControl(-1),
    name: new FormControl(''),
    price: new FormControl(0)
  });

  productId: number;
  product: Product;

  constructor(
    private prodService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  ngOnInit() {
    this.productId = +this.route.snapshot.paramMap.get('id');

    this.prodService.get(this.productId).subscribe((product: Product) => {
      this.product = product;
      this.productForm.setValue(this.product);
    });
  }

  onSaveBtnClicked() {
    if (this.productId) {
      this.prodService
        .update(this.productId, this.productForm.value)
        .subscribe(() => {
          this.router.navigate(['/products']);
        });
    } else {
      this.prodService.add(this.productForm.value).subscribe(() => {
        this.router.navigate(['/products']);
      });
    }
  }
}
