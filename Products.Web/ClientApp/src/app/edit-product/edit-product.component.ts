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
    id: new FormControl(),
    name: new FormControl(),
    price: new FormControl(),
    createdOn: new FormControl(),
    modifiedOn: new FormControl()
  });

  productId: number;
  isLoading = true;
  errMsg = '';

  constructor(
    private prodService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  ngOnInit() {
    this.productId = +this.route.snapshot.paramMap.get('id');

    if (this.productId) {
      this.prodService.get(this.productId).subscribe((product: Product) => {
        this.isLoading = false;
        this.productForm.setValue(product);
      });
    } else {
      this.isLoading = false;
      this.productForm.reset();
    }
  }

  onSaveBtnClicked() {
    this.errMsg = '';

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
