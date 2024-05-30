import { Component, Input, OnInit, ViewChild, inject } from '@angular/core';
import { TurbineReadViewModel } from '../../../../../assets/models/turbine.read.viewmodel';
import { JsonPipe, NgIf } from '@angular/common';
import {
  NgApexchartsModule,
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexYAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexStroke
} from "ng-apexcharts";
import { TurbineDataReadViewModel } from '../../../../../assets/models/turbine.data.read.viewmodel';
import { TurbineService } from '../../../../services/turbine.service';
import { TurbineStatus } from '../../../../../assets/enums/turbine.status';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { TurbinesDeleteComponent } from '../turbines.delete/turbines.delete.component';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-turbines-details',
  standalone: true,
  imports: [
    JsonPipe,
    NgApexchartsModule,
    MatButtonModule,
    NgIf
  ],
  templateUrl: './turbines.details.component.html',
  styleUrl: './turbines.details.component.scss'
})
export class TurbinesDetailsComponent implements OnInit {
  private turbineService = inject(TurbineService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  @Input() turbine!: TurbineReadViewModel;
  TurbineStatus = TurbineStatus;

  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions!: Partial<ChartOptions>;

  isAdmin: boolean = false;

  turbineData: TurbineDataReadViewModel[] = [];

  ngOnInit(): void {
    this.isAdmin = this.authService.getIsAdmin();

    this.turbineService.getDataHistorical(this.turbine.id, new Date('2024-01-01'), new Date('2024-06-01'))
      .subscribe(result => {
        this.turbineData = result.sort((a, b) =>{
          if(!a.dateTime || a.dateTime < b.dateTime)
            return -1
          else if (!b.dateTime || a.dateTime > b.dateTime)
            return 1
          return 0
      });
        this.updateChart(this.turbineData);
      });
  }

  onUpdateTurbine() {
    this.router.navigate(['farms', this.turbine.windFarmId, 'turbines', 'update', this.turbine.id]);
  }

  onDeleteTurbine(): void {
    const dialogRef = this.dialog.open(TurbinesDeleteComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.turbineService.delete(this.turbine.id)
          .subscribe(() => {
            this.router.navigate(['farms'], { skipLocationChange: true })
              .then(() => this.router.navigate(['farms', this.turbine.windFarmId]));
          });
      }
    });
  }

  updateChart(data: TurbineDataReadViewModel[]){
    this.chartOptions = {
      series: [
        {
          name: "Rated Power",
          data: data.map(d => d.ratedPower),
          color: '#3F51B5'
        }
      ],
      chart: {
        height: 350,
        type: "area",
        toolbar: {
          show: true,
          offsetX: 0,
          offsetY: 0,
          tools: {
            download: true,
            selection: false,
            zoom: false,
            zoomin: true,
            zoomout: true,
            pan: false,
            reset: false,
            customIcons: []
          },
          autoSelected: 'zoom' 
        },
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        type: "datetime",
        categories: data.map(d => d.dateTime)
      },
      yaxis: {
        labels: {
          formatter: function (val) {
            return val.toFixed(0);
          }
        }
      },
      tooltip: {
        x: {
          format: "dd/MM/yy HH:mm"
        }
      }
    };
  }
}

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
};
