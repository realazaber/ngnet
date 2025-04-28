import { Component } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { RegisterDTO } from '../../models/auth/user/register.dto';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  imports: [SharedModule, FormsModule],
})
export class RegisterPage {
  public formData: RegisterDTO = {} as RegisterDTO;

  constructor(private authService: AuthService) {}

  public register() {
    if (
      !this.formData.email ||
      !this.formData.firstName ||
      !this.formData.lastName ||
      !this.formData.password ||
      !this.formData.confirmPassword
    ) {
      alert('Please fill in all fields');
      return;
    }

    if (this.formData.password !== this.formData.confirmPassword) {
      alert('Passwords do not match');
      return;
    }

    this.authService.register(this.formData).subscribe(() => {
      alert('User registered successfully');
    });
  }
}
