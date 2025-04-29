import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContainerComponent } from '../components/container/container.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule, ContainerComponent],
  exports: [RouterModule, ContainerComponent],
})
export class SharedModule {}
