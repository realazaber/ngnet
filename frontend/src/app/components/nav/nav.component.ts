import { Component, Input, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../models/auth/user/user.dto';
import { filter } from 'rxjs';

@Component({
  selector: 'ngnet-nav',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './nav.component.html',  
})
export class NavComponent implements OnInit {
  authenticated: boolean = false;
  manageUsers: boolean = false;
  dmsAccess: boolean = false;

  currentUser: UserDTO = {} as UserDTO;

  private previousIsAuthRoute: boolean = false; // Tracks whether the last route was 'auth'

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {
    authService.isLoggedIn$.subscribe((status) => {
      this.authenticated = status;
    });
  }

  ngOnInit() {
    this.loadLinks(); // Load on initial navigation

    // Listen for navigation changes
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const isAuthRoute = event.urlAfterRedirects.startsWith('/auth');

        // Reload only if we switch between auth and non-auth routes
        if (isAuthRoute !== this.previousIsAuthRoute) {
          this.loadLinks();
        }

        this.previousIsAuthRoute = isAuthRoute; // Update the current route type
      });
  }

  loadLinks(): void {
    this.currentUser.roles = [];
    this.manageUsers = false;
    this.dmsAccess = false;
    this.authenticated = this.authService.getAccessToken() != '';
    this.userService.getCurrentUser().subscribe((data: UserDTO) => {
      this.currentUser = data;
      if (
        this.currentUser.roles.includes('Admin') ||
        this.currentUser.roles.includes('ManageUsers')
      ) {
        this.manageUsers = true;
      }
      if (this.currentUser.roles.includes('Dms')) {
        this.dmsAccess = true;
      }
    });
  }

  logout(): void {
    this.authenticated = false;
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
