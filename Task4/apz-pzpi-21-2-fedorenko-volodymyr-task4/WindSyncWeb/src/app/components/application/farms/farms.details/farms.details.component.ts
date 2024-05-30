import { Component, OnInit, inject } from '@angular/core';
import { FarmService } from '../../../../services/farm.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FarmReadViewModel } from '../../../../../assets/models/farm.read.viewmodel';
import { CommonModule, JsonPipe } from '@angular/common';
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
    TurbinesDetailsComponent
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
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  dialog = inject(MatDialog);

  farmId?: number;
  farm!: FarmReadViewModel;
  turbines: TurbineReadViewModel[] = [];
  TurbineStatus = TurbineStatus;

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
    this.farmId = this.route.snapshot.params['id'] as number;

    this.farmService.getById(this.farmId)
      .subscribe(result => {
        this.farm = result;
      });

    this.farmService.getTurbines(this.farmId)
      .subscribe(result => {
        this.turbines = result;
      });
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
