import { Component, Input, output } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { TableVieModel } from './models/TableVieModel';
import { DynamicPipe } from '../pipes/dynamic.pipe';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { NzFloatButtonModule } from 'ng-zorro-antd/float-button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-table',
  imports: [NzTableModule, NzButtonModule, DynamicPipe, NzFlexModule, NzIconModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {

  @Input() columns: any;
  @Input() rows: any;
  @Input() table!: TableVieModel;

  showTable: boolean = false;
  deleteChange = output<any>();
  updateChange = output<any>();

  updateRow(row: any){
    this.updateChange.emit(row);
  }

  deleteRow(row:any){
    this.deleteChange.emit(row)
  }
}
