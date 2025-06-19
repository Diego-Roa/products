import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Product } from '../interfaces/product.interface';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  private readonly apiUrl: string = environment.apiUrl + "/products";

  constructor(private readonly http: HttpClient) { }

  public getProducts(){
    return this.http.get<any>(`${this.apiUrl}`)
  }

  public createProduct(request: Product){
    return this.http.post<any>(`${this.apiUrl}`, request)
  }

  public updateProduct(request: Product){
    return this.http.put<any>(`${this.apiUrl}/${request.id}`, request)
  }

  public deleteProducts(request: Product){
    return this.http.delete<any>(`${this.apiUrl}/${request.id}`)
  }
}
