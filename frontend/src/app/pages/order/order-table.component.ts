import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { orderColumnsConstants } from 'src/app/constants/order.columns.constants';
import { Order } from 'src/app/interfaces/order';
import { ModalOpenOrderComponent } from 'src/app/modal-open-order/modal-open-order.component';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'order-table',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './order-table.component.html',
  styleUrls: ['./order-table.component.scss']
})


export class OrderTableComponent implements OnInit {
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    if (mp) {
      this.paginator = mp;
      this.dataSource.paginator = this.paginator;
    }
  }
  @ViewChild(MatSort) sort: MatSort | undefined;
  private paginator: MatPaginator;


  /** источник данных */
  dataSource: MatTableDataSource<Order>; // источник данных
  /** общий массив данных */
  ordersData: Order[] = [];


  readonly displayedColumns = orderColumnsConstants.displayedColumns;

  constructor(
    private dialog: MatDialog,
    private fakeBackendService: FakeBackendService) {
  }

  ngOnInit(): void {
    // this.fakeBackendService.orders$.subscribe((item:any) => {
    //   console.log('item ', item)
    //   if (item?.value != null) {
    //     if (!!this.dataSource?.data) {
    //       this.dataSource.data = item.value;
    //     } else {
    //       this.dataSource = new MatTableDataSource<Order>(item.value);
    //     }
    //     this.dataSource.paginator = this.paginator;
    //     this.dataSource.sort = this.sort!;
    //     this.ordersData = item.value; 
    //   }
    // });
    let data = this.fakeBackendService.orders;
    if (data.length > 0) {
      console.log('data ', data)
      this.dataSource = new MatTableDataSource<Order>(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort!;
      this.ordersData = data;
    }
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

  /** Announce the change in sort state for assistive technology. */
  announceSortChange(sortState: Sort | any) {
  }

  /** Создать заявку. */
  createOrder() {
    this.dialog.open(ModalOpenOrderComponent, {
      width: '550',
      data: {}
    });
  }

  /** Посмотреть заявку. */
  lookAtOrder(order: Order) {
    this.dialog.open(ModalOpenOrderComponent, {
      width: '550',
      data: order
    });
  }

}