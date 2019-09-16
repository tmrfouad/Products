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
import { MatDialog, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements AfterViewInit {
  products: Product[];
  @ViewChild('SearchInput') searchInput: ElementRef;

  constructor(private prodService: ProductService, private dialog: MatDialog) {}

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
    const dialogRef = this.dialog.open(RemoveProductDielogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.prodService.delete(productId).subscribe(() => {
          this.products = this.products.filter(p => p.id !== productId);
        });
      }
    });
  }
}

@Component({
  selector: 'app-dialog-overview-example-dialog',
  template: `
    <h1 mat-dialog-title>Confirm</h1>
    <div mat-dialog-content>
      <p>Are you sure want to remove this product?</p>
    </div>
    <div mat-dialog-actions>
      <button
        class="btn btn-secondary ml-1"
        [mat-dialog-close]="'yes'"
        cdkFocusInitial
      >
        Yes
      </button>
      <button class="btn btn-secondary ml-1" (click)="onNoClick()">No</button>
    </div>
  `
})
export class RemoveProductDielogComponent {
  constructor(public dialogRef: MatDialogRef<RemoveProductDielogComponent>) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
