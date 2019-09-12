import { Component, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import {
  map,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  startWith
} from 'rxjs/operators';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements AfterViewInit {
  products: Product[];
  @ViewChild('SearchInput') searchInput: ElementRef;

  constructor(private prodService: ProductService, private router: Router) {}

  ngAfterViewInit(): void {
    fromEvent<any>(this.searchInput.nativeElement, 'keyup')
      .pipe(
        map(event => event.target.value),
        startWith(''),
        debounceTime(400),
        distinctUntilChanged(),
        switchMap(search => this.prodService.search(search))
      )
      .subscribe((products: Product[]) => {
        this.products = products;
      });
  }

  onRemoveBtnClicked(productId: number) {
    this.prodService.delete(productId).subscribe(() => {
      this.products = this.products.filter(p => p.id !== productId);
    });
  }
}
