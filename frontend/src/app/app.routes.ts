import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home.component';
import { LoginPage } from './pages/login/login.component';
import { RegisterPage } from './pages/register/register.component';
import { ErrorPage } from './pages/error/error.component';
import { DashboardPage } from './pages/auth/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { ProfilePage } from './pages/auth/profile/profile.component';
import { ViewUsersPage } from './pages/auth/user/view/manageusers.component';
import { ManageUsersGuard } from './guards/manageusers.guard';
import { CreateUserPage } from './pages/auth/user/create/createuser.component';
import { DmsPage } from './pages/auth/dms/dms.component';
import { dmsGuard } from './guards/dms.guard';
import { EditUserPage } from './pages/auth/user/edit/edit.component';
import { ManageRolesPage } from './pages/auth/manageroles/manageroles.component';

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
    path: 'auth/dashboard',
    component: DashboardPage,
    pathMatch: 'full',
    canActivate: [authGuard],
  },
  {
    path: 'auth/dms',
    component: DmsPage,
    pathMatch: 'full',
    canActivate: [authGuard, dmsGuard],
  },
  {
    path: 'auth/createuser',
    component: CreateUserPage,
    pathMatch: 'full',
    canActivate: [authGuard, ManageUsersGuard],
  },
  {
    path: 'auth/edituser/:id',
    component: EditUserPage,
    pathMatch: 'full',
    canActivate: [authGuard, ManageUsersGuard],
  },
  {
    path: 'auth/viewusers',
    component: ViewUsersPage,
    pathMatch: 'full',
    canActivate: [authGuard, ManageUsersGuard],
  },
  {
    path: 'auth/manageroles',
    component: ManageRolesPage,
    pathMatch: 'full',
    canActivate: [authGuard],
  },
  {
    path: 'auth/profile',
    component: ProfilePage,
    pathMatch: 'full',
    canActivate: [authGuard],
  },
  {
    path: '**',
    component: ErrorPage,
    pathMatch: 'full',
  },
];
