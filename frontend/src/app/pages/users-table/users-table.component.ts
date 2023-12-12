import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { userColumnsConstants } from 'src/app/constants/user.columns.constants';
import { User } from 'src/app/interfaces/user';
import { ModalOpenUserComponent } from 'src/app/modal-open-user/modal-open-user.component';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'users-table',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})


export class UsersTableComponent implements OnInit {
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    if (mp) {
      this.paginator = mp;
      this.dataSource.paginator = this.paginator;
    }
  }
  @ViewChild(MatSort) sort: MatSort | undefined;
  private paginator: MatPaginator;

  /** Названия полей пользователя */
  userColumnNames = userColumnsConstants.labelColumns;
  /** Названия полей статуса пользователя */
  states = userColumnsConstants.states;

  /** источник данных */
  dataSource: MatTableDataSource<User>; // источник данных
  /** общий массив данных */
  usersData: User[] = [];


  readonly displayedColumns = userColumnsConstants.displayedColumns;

  constructor(
    private dialog: MatDialog,
    private fakeBackendService: FakeBackendService) {
  }

  ngOnInit(): void {
    let data = this.fakeBackendService.usersData;
    if (data.length > 0) {
      this.dataSource = new MatTableDataSource<User>(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort!;
      this.usersData = data;
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
  createUser() {
    this.dialog.open(ModalOpenUserComponent, {
      width: '550',
      data: {}
    });
  }

  /** Посмотреть заявку. */
  lookAtUser(user: User) {
    this.dialog.open(ModalOpenUserComponent, {
      width: '550',
      data: user
    });
  }

}