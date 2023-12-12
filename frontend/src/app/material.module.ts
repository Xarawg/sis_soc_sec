
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule, MatOptionModule, MatRippleModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatTableModule} from '@angular/material/table';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatSelectModule} from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatListModule} from '@angular/material/list';
import {MatCardModule} from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import {MatDialogModule} from '@angular/material/dialog';
import { NgModule } from '@angular/core';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';
import {MatGridListModule} from '@angular/material/grid-list';

const MATERIAL_MODULES = [
  MatButtonModule,
  MatNativeDateModule,
  MatRippleModule,
  MatCardModule,
  MatDatepickerModule,
  MatFormFieldModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatProgressSpinnerModule,
  MatRippleModule,
  MatSelectModule,
  MatNativeDateModule,
  MatTableModule,
  MatPaginatorModule,
  MatDialogModule,
  MatSlideToggleModule,
  MatToolbarModule,
  MatSortModule,
  MatMenuModule,
  MatGridListModule,
  MatOptionModule
]

@NgModule({
    imports: [
      MATERIAL_MODULES
    ],
    exports: [
      MATERIAL_MODULES
    ]
  })

export class MaterialModule { }