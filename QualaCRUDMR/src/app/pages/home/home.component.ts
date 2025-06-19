import { Component, OnInit } from '@angular/core';
import { TableComponent } from "../../shared/table/table.component";
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TableVieModel } from '../../shared/table/models/TableVieModel';
import { Product } from '../../interfaces/product.interface';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-home',
  imports: [
    TableComponent,
    NzIconModule,
    NzButtonModule,
    NzInputModule,
    NzFlexModule,
    FormsModule,
    NzModalModule,
    ReactiveFormsModule,
    NzFormModule,
    NzDatePickerModule,
    NzSelectModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent implements OnInit {
  table!: TableVieModel;
  showTable: boolean = false;

  products: Product[] = [];

  productForm!: FormGroup;

  searchText = '';

  isVisibleForm: boolean = false;

  titleForm: string = 'Hola';

  isOkLoading: boolean = false;

  constructor(private readonly productsService: ProductsService,
    private fb: FormBuilder
  ) {
    this.productForm = fb.group({
      id: new FormControl(''),
      name: new FormControl('', [Validators.required, Validators.maxLength(250)]),
      description: new FormControl('', [Validators.required, Validators.maxLength(250)]),
      reference: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      unitPrice: new FormControl(null, [Validators.required, Validators.pattern(/^\d+(\.\d+)?$/)]),
      status: new FormControl('', Validators.required),
      unitMeasurement: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      createdAt: new FormControl('', Validators.required)
    })
  }

  ngOnInit() {
    this.getProducts();
  }

  getProducts(): void {
    this.productsService.getProducts().subscribe({
      next: (response: any) => {
        this.products = response;
        this.table = {
          columns: [
            { key: 'id', value: 'Código Producto' },
            { key: 'name', value: 'Nombre' },
            { key: 'description', value: 'Descripción' },
            { key: 'reference', value: 'Referencia interna' },
            { key: 'unitPrice', value: 'Precio unitario', pipe: 'currency' },
            { key: 'status', value: 'Estado' },
            { key: 'unitMeasurement', value: 'Unidad de medida' },
            { key: 'createdAt', value: 'Fecha de creación', pipe: 'date', pipeArgs: ['dd/MM/yyyy'] },
          ],
          data: this.products,
          showActions: true
        };
        this.showTable = true;
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  deleteProduct(selectProduct: Product): void {
    this.productsService.deleteProducts(selectProduct).subscribe({
      next: (response) => {
        this.getProducts();
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  updateProductSignal(selectProduct: Product): void {
    this.productForm.patchValue({
      id: selectProduct.id,
      name: selectProduct.name,
      description: selectProduct.description,
      reference: selectProduct.reference,
      unitPrice: selectProduct.unitPrice,
      status: selectProduct.status,
      unitMeasurement: selectProduct.unitMeasurement,
      createdAt: selectProduct.createdAt,
    });
    this.isVisibleForm = true;
  }

  /**
   * Filter by any column of the table
   */
  search(): void {
    const value = this.searchText.trim().toLowerCase();
    this.table.data = this.products.filter(row =>
      this.table.columns.some(col => {
        const cellValue = row[col.key as keyof Product];
        if (cellValue === null || cellValue === undefined) return false;
        return String(cellValue).toLowerCase().includes(value);
      })
    );
  }

  showModalToCreate(): void {
    this.productForm.patchValue({
      id: '',
      name: '',
      description: '',
      reference: '',
      unitPrice: '',
      status: '',
      unitMeasurement: '',
      createdAt: '',
    });
    this.isVisibleForm = true;
  }

  closeModal(): void {
    this.isVisibleForm = false;
  }

  disableAfterToday = (current: Date): boolean => {
    return current > new Date();
  }

  onSubmit(): void {
    this.isOkLoading = true;
    if (!this.productForm.valid) {
      this.isOkLoading = false;
      return;
    }
    this.productForm.get('id')?.value != '' ?  this.updateProduct(this.productForm.value) : this.createProduct(this.productForm.value)
  }

  createProduct(newProduct: Product): void {
    this.productsService.createProduct(newProduct).subscribe({
      next: (response) => {
        this.getProducts();
        this.isOkLoading = false;
        this.isVisibleForm = false;
      },
      error: (error) => {
        console.error(error);
        this.isOkLoading = false;
      }
    })
  }

  updateProduct(updatedProduct: Product): void {
    this.productsService.updateProduct(updatedProduct).subscribe({
      next: (response) => {
        this.getProducts();
        this.isOkLoading = false;
        this.isVisibleForm = false;
      },
      error: (error) => {
        console.error(error);
        this.isOkLoading = false;
      }
    })
  }

}
