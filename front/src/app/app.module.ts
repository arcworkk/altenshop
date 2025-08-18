import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TagModule } from 'primeng/tag';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    ButtonModule,
    TableModule,
    PaginatorModule,
    InputTextModule,
    ToastModule,
    DialogModule,
    ConfirmDialogModule,
    TagModule,
    DropdownModule,
    TooltipModule,
    CardModule
  ]
})
export class AppModule {}