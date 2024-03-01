import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { userColumnsConstants } from 'src/app/constants/user.columns.constants';
import { User } from 'src/app/interfaces/user';
import { ModalOpenUserComponent } from 'src/app/modal-open-user/modal-open-user.component';
import { HttpService } from 'src/app/services/http.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'users-table',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss'],
})


export class UsersTableComponent implements OnInit, AfterViewInit {
  /** источник данных */
  dataSource: MatTableDataSource<User> = new MatTableDataSource();
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    if (mp) {
      this.paginator = mp;
      this.dataSource.paginator = this.paginator;
    }
  }
  @ViewChild(MatSort) sort: MatSort | null;
  private paginator: MatPaginator;

  /** Названия полей пользователя */
  userColumnNames = userColumnsConstants.labelColumns;
  /** Названия полей статуса пользователя */
  states = userColumnsConstants.states;
  /** общий массив данных */
  usersData: User[] = [];

  /** Отображаемые в таблице колонки */
  readonly displayedColumns = userColumnsConstants.displayedColumns;

  constructor(
    private dialog: MatDialog,
    private httpService: HttpService,
    private changeDetectorRefs: ChangeDetectorRef,
    private modalService: ModalService) {
  }
  
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  ngOnInit(): void {
    this.updateUserDataList();
    this.modalService.changed.subscribe( (data:any) => {
      this.updateUserDataList();
    });
    this.changeDetectorRefs.detectChanges();
  }

  private updateUserDataList(){
    this.httpService.getUsers()
    .subscribe( (data:any) => {
      const res = data.value;
      this.dataSource = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort!;
      this.usersData = res;
    });
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

  /** Создать пользователя. */
  createUser() {
    this.dialog.open(ModalOpenUserComponent, {
      height: "calc(100% - 100px)",
      width: "calc(100% - 100px)",
      maxWidth: "100%",
      maxHeight: "100%",
      data: {}
    });
  }

  /** Посмотреть пользователя. */
  lookAtUser(user: User) {  
    this.dialog.open(ModalOpenUserComponent, {
      height: "calc(100% - 100px)",
      width: "calc(100% - 100px)",
      maxWidth: "100%",
      maxHeight: "100%",
      data: user
    });
  }
}