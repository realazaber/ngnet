import { Component, OnInit } from '@angular/core';
import { UserDTO } from '../../../models/auth/user/user.dto';
import { UserService } from '../../../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { SharedModule } from '../../../shared/shared.module';
import { EditUserComponent } from '../../../components/auth/user/edit/edit.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterModule, SharedModule, EditUserComponent],
  templateUrl: './dashboard.component.html',
  styles: ``,
})
export class DashboardPage implements OnInit {
  currentUser: UserDTO = {} as UserDTO;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit() {
    this.userService.getCurrentUser().subscribe((user: UserDTO) => {
      console.log('User: ' + user);

      this.currentUser = user;
      console.log('Current User: ' + this.currentUser.id);

      if (this.currentUser == null) {
        this.router.navigate(['/']);
      }
    });
  }
}
