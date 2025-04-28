import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [SharedModule, RouterModule],
  templateUrl: './error.component.html',
  styles: ``,
})
export class ErrorPage {
  constructor(private router: Router) {}
}
