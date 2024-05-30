import { Component, Input, OnInit, ViewChild, inject } from '@angular/core';
import { TurbineReadViewModel } from '../../../../../assets/models/turbine.read.viewmodel';
import { JsonPipe } from '@angular/common';
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

@Component({
  selector: 'app-turbines-details',
  standalone: true,
  imports: [
    JsonPipe,
    NgApexchartsModule,
    MatButtonModule
  ],
  templateUrl: './turbines.details.component.html',
  styleUrl: './turbines.details.component.scss'
})
export class TurbinesDetailsComponent implements OnInit {
  turbineService = inject(TurbineService);

  @Input() turbine!: TurbineReadViewModel;
  TurbineStatus = TurbineStatus;

  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions!: Partial<ChartOptions>;

  turbineData: TurbineDataReadViewModel[] = [];

  ngOnInit(): void {
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
