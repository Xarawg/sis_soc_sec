import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { orderColumnsConstants } from 'src/app/constants/order.columns.constants';
import { Order } from 'src/app/interfaces/order';
import { ModalOpenOrderComponent } from 'src/app/modal-open-order/modal-open-order.component';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'orders-table',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './orders-table.component.html',
  styleUrls: ['./orders-table.component.scss']
})


export class OrderTableComponent implements OnInit {
  /** источник данных */
  dataSource: MatTableDataSource<Order> = new MatTableDataSource();
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    if (mp) {
      this.paginator = mp;
      this.dataSource.paginator = this.paginator;
    }
  }
  @ViewChild(MatSort) sort: MatSort | null;
  private paginator: MatPaginator;

  /** общий массив данных */
  ordersData: Order[] = [];

  /** Отображаемые в таблице колонки */
  readonly displayedColumns = orderColumnsConstants.displayedColumns;

  constructor(
    private dialog: MatDialog,
    private httpService: HttpService,
    private changeDetectorRefs: ChangeDetectorRef) {
  }
  
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  ngOnInit(): void {
    this.updateOrdersDataList();
  }

  private updateOrdersDataList(){
    this.httpService.getOrders().subscribe( (data:any)=> {
      const res = data.value;
      this.dataSource = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort!;
      this.ordersData = res;
    });
    this.changeDetectorRefs.detectChanges();
  }

  /** Применить фильтр */
  applyFilter(event: any) {
    if (event) {
      let filterValue = event.target?.value;
      filterValue = filterValue.trim(); // Remove whitespace
      filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
      this.dataSource.filter = filterValue;
    }
  }

  /** Создать заявку. */
  createOrder() {
    this.dialog.open(ModalOpenOrderComponent, {
      height: "calc(100% - 100px)",
      width: "calc(100% - 100px)",
      maxWidth: "100%",
      maxHeight: "100%",
      data: {}
    });
  }

  /** Посмотреть заявку. */
  lookAtOrder(order: Order) {
    this.dialog.open(ModalOpenOrderComponent, {
      height: "calc(100% - 100px)",
      width: "calc(100% - 100px)",
      maxWidth: "100%",
      maxHeight: "100%",
      data: order
    });
  }

}