import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { authGuard } from './guards/auth.guard';
import { HomeComponent } from './components/application/home/home.component';
import { FarmsViewComponent } from './components/application/farms/farms.view/farms.view.component';
import { FarmsAddComponent } from './components/application/farms/farms.add/farms.add.component';
import { FarmsDetailsComponent } from './components/application/farms/farms.details/farms.details.component';
import { TurbinesAddComponent } from './components/application/turbines/turbines.add/turbines.add.component';
import { TurbinesViewComponent } from './components/application/turbines/turbines.view/turbines.view.component';

export const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full'},
    { path: 'auth', children: [
        { path: 'login', component: LoginComponent }
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
    ]}
];
