import { Component, inject } from '@angular/core';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { TurbineService } from '../../../../services/turbine.service';
import { TurbineReadViewModel } from '../../../../../assets/models/turbine.read.viewmodel';
import { TurbinesDetailsComponent } from '../turbines.details/turbines.details.component';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-turbines.view',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    CommonModule,
    TurbinesDetailsComponent
  ],
  templateUrl: './turbines.view.component.html',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  styleUrl: './turbines.view.component.scss'
})
export class TurbinesViewComponent {
  private turbineService = inject(TurbineService);
  private authService = inject(AuthService);

  isAdmin: boolean = false;

  turbines: TurbineReadViewModel[] = [];

  turbinesDisplayedColumns: string[] = ['id', 'turbineRadius', 'sweptArea', 'latitude', 'longitude', 'altitude', 'efficiency'];
  turbinesColumnNames: { [key: string]: string } = {
    'id': 'Id',
    'turbineRadius': 'TurbineRadius',
    'sweptArea': 'SweptArea',
    'latitude': 'Latitude',
    'longitude': 'Longitude',
    'altitude': 'Altitude',
    'efficiency': 'Efficiency',
  };
  turbinesDisplayedColumnsWithExpand = [...this.turbinesDisplayedColumns, 'expand'];
  expandedTurbine?: TurbineReadViewModel;

  ngOnInit(): void {
    this.isAdmin = this.authService.getIsAdmin();

    if(this.isAdmin){
      this.turbineService.getAll()
        .subscribe(result => {
          this.turbines = result;
        });
    }
    else {
      this.turbineService.getMy()
        .subscribe(result => {
          this.turbines = result;
        });
    }
  }
}
