import { Component, OnInit } from '@angular/core';
import { ContainerComponent } from '../../../components/container/container.component';
import { SharedModule } from '../../../shared/shared.module';
import { UserService } from '../../../services/user.service';
import { UserDTO } from '../../../models/auth/user/user.dto';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [SharedModule, FormsModule],
  templateUrl: './profile.component.html',
  styles: ``,
})
export class ProfilePage implements OnInit {
  currentUser: UserDTO = {} as UserDTO;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((user: UserDTO) => {
      this.currentUser = user;
    });
  }

  update(): void {
    if (
      this.currentUser.firstName == '' ||
      this.currentUser.lastName == '' ||
      this.currentUser.email == '' ||
      this.currentUser.phone == null
    ) {
      alert('Fill in all required fields');
      return;
    }
  }
}
