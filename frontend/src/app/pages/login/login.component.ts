import { Component } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { LoginDTO } from '../../models/auth/user/login.dto';
import { AuthService } from '../../services/auth.service';
import { TokenDTO } from '../../models/auth/token.dto';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { error } from 'console';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',  
})
export class LoginPage {
  public formData: LoginDTO = {} as LoginDTO;

  constructor(private authService: AuthService, private router: Router) {}

  async login() {
    if (!this.formData.email || !this.formData.password) {
      alert('Please fill in all fields');
    }

    this.authService.login(this.formData).subscribe((token: TokenDTO) => {
      this.authService.saveToken(token);
      this.router.navigateByUrl('auth/dashboard');
    });
  }
}
