import { Component, OnInit, inject } from '@angular/core';
import { FarmService } from '../../../../services/farm.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FarmReadViewModel } from '../../../../../assets/models/farm.read.viewmodel';
import { CommonModule, JsonPipe, NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { TurbineReadViewModel } from '../../../../../assets/models/turbine.read.viewmodel';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { TurbineStatus } from '../../../../../assets/enums/turbine.status';
import { TurbinesDetailsComponent } from '../../turbines/turbines.details/turbines.details.component';
import { MatDialog } from '@angular/material/dialog';
import { FarmsDeleteComponent } from '../farms.delete/farms.delete.component';
import { TurbineService } from '../../../../services/turbine.service';
import { TurbineDataReadViewModel } from '../../../../../assets/models/turbine.data.read.viewmodel';
import { forkJoin } from 'rxjs';
import {MatDividerModule} from '@angular/material/divider';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-farms.details',
  standalone: true,
  imports: [
    JsonPipe,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    CommonModule,
    TurbinesDetailsComponent,
    MatDividerModule,
    NgIf
  ],
  templateUrl: './farms.details.component.html',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  styleUrl: './farms.details.component.scss'
})
export class FarmsDetailsComponent implements OnInit {
  private farmService = inject(FarmService);
  private authService = inject(AuthService);
  private turbineService = inject(TurbineService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);

  isAdmin: boolean = false;

  farmId!: number;
  farm!: FarmReadViewModel;
  turbines: TurbineReadViewModel[] = [];
  operationalTurbinesCount: number = 0;

  turbinesCombinedData: TurbineDataReadViewModel = {
    id: 0,
    dateTime: new Date(),
    windSpeed: 0,
    ratedPower: 0,
    powerOutput: 0,
    airDensity: 0,
    airPressure: 0,
    airTemperature: 0
  };

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
    this.farmId = this.route.snapshot.params['farm-id'] as number;

    this.isAdmin = this.authService.getIsAdmin();

    this.farmService.getById(this.farmId)
      .subscribe(result => {
        this.farm = result;
      });

    this.farmService.getTurbines(this.farmId)
      .subscribe(result => {
        this.turbines = result;
        this.operationalTurbinesCount = this.turbines.filter(t => t.status === 1).length;
        this.getStats();
      });
  }

  getStats() {
    const today = new Date();
    const start = new Date(today.setHours(0, 0, 0, 0));
    const end = new Date(today.setHours(23, 59, 59, 999));

    this.farmService.getTurbines(this.farmId).subscribe(turbines => {
      this.turbines = turbines;
  
      const turbineDataRequests = this.turbines.map(turbine => {
        return this.turbineService.getDataHistorical(turbine.id, start, end);
      });
  
      forkJoin(turbineDataRequests).subscribe(allData => {
        const totalDataPoints = allData.reduce((acc, data) => acc + data.length, 0);
  
        let totalWindSpeed = 0;
        let totalRatedPower = 0;
        let totalPowerOutput = 0;

        allData.forEach(data => {
          data.forEach(d => {
            totalWindSpeed += d.windSpeed;
            totalRatedPower += d.ratedPower;
            totalPowerOutput += d.powerOutput;
          });
        });
  
        this.turbinesCombinedData.windSpeed = totalWindSpeed / totalDataPoints;
        this.turbinesCombinedData.ratedPower = Number(((totalRatedPower / totalDataPoints) / 1000000).toFixed(3));
        this.turbinesCombinedData.powerOutput = Number((totalPowerOutput / 1000000).toFixed(3));
      });
    });
  }

  onAddTurbine() {
    this.router.navigate(['farms', this.farmId, 'turbines', 'add']);
  }

  onFarmUpdate() {
    this.router.navigate(['farms', 'update', this.farmId]);
  }

  onFarmDelete(): void {
    const dialogRef = this.dialog.open(FarmsDeleteComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.farmService.delete(this.farmId!)
          .subscribe(() => {
            this.router.navigate(['farms']);
          });
      }
    });
  }
}
