import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home.component';
import { LoginPage } from './pages/login/login.component';
import { RegisterPage } from './pages/register/register.component';
import { ErrorPage } from './pages/error/error.component';
import { DashboardPage } from './pages/auth/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { ProfilePage } from './pages/auth/profile/profile.component';
import { ManageusersPage } from './pages/auth/manageusers/manageusers.component';
import { ManageUsersGuard } from './guards/manageusers.guard';
import { CreateUserPage } from './pages/auth/createuser/createuser.component';

export const routes: Routes = [
  {
    path: '',
    component: HomePage,
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginPage,
    pathMatch: 'full',
  },
  {
    path: 'register',
    component: RegisterPage,
    pathMatch: 'full',
  },

  {
    path: 'dashboard',
    component: DashboardPage,
    pathMatch: 'full',
    canActivate: [authGuard],
  },
  {
    path: 'createuser',
    component: CreateUserPage,
    pathMatch: 'full',
    canActivate: [authGuard, ManageUsersGuard],
  },
  {
    path: 'profile',
    component: ProfilePage,
    pathMatch: 'full',
    canActivate: [authGuard],
  },
  {
    path: 'manageusers',
    component: ManageusersPage,
    pathMatch: 'full',
    canActivate: [authGuard, ManageUsersGuard],
  },
  {
    path: '**',
    component: ErrorPage,
    pathMatch: 'full',
  },
];
