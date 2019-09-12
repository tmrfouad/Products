import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}api/products`);
  }

  search(search: string): Observable<Product[]> {
    return this.http.get<Product[]>(
      `${this.baseUrl}api/products/search/${search}`
    );
  }

  get(productId: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}api/products/${productId}`);
  }

  add(product: Product): Observable<Product> {
    return this.http.post<Product>(`${this.baseUrl}api/products`, product);
  }

  update(productId: number, updates: Product): Observable<Product> {
    return this.http.patch<Product>(
      `${this.baseUrl}api/products/${productId}`,
      updates
    );
  }

  delete(productId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}api/products/${productId}`);
  }
}
