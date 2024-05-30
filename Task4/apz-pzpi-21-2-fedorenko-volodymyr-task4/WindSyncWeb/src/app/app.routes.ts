import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { authGuard } from './guards/auth.guard';
import { HomeComponent } from './components/application/home/home.component';
import { FarmsViewComponent } from './components/application/farms/farms.view/farms.view.component';
import { FarmsAddComponent } from './components/application/farms/farms.add/farms.add.component';
import { FarmsDetailsComponent } from './components/application/farms/farms.details/farms.details.component';
import { TurbinesAddComponent } from './components/application/turbines/turbines.add/turbines.add.component';
import { TurbinesViewComponent } from './components/application/turbines/turbines.view/turbines.view.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { AdminsViewComponent } from './components/admin/admins.view/admins.view.component';
import { AdminsAddComponent } from './components/admin/admins.add/admins.add.component';
import { UsersViewComponent } from './components/admin/users.view/users.view.component';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full'},
    { path: 'auth', children: [
        { path: 'login', component: LoginComponent },
        { path: 'register', component: RegisterComponent }
    ]},
    { path: 'farms', canActivate: [authGuard], children: [
        { path: '', component: FarmsViewComponent},
        { path: 'add', component: FarmsAddComponent},
        { path: 'update/:id', component: FarmsAddComponent},
        { path: ':farm-id', children: [
            { path: '', component: FarmsDetailsComponent },
            { path: 'turbines/add', component: TurbinesAddComponent },
            { path: 'turbines/update/:id', component: TurbinesAddComponent }
        ]}
    ]},
    { path: 'turbines', canActivate: [authGuard], children: [
        { path: '', component: TurbinesViewComponent }
    ]},
    { path: 'admin', canActivate: [authGuard, adminGuard], children: [
        { path: 'users', component: UsersViewComponent },
        { path: 'admins', children: [
            { path: '', component: AdminsViewComponent},
            { path: 'add', component: AdminsAddComponent }
        ]}
    ]}
];
