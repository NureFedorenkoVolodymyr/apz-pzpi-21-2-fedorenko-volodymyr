import { Component, OnInit, inject } from '@angular/core';
import { FarmService } from '../../../../services/farm.service';
import { FarmReadViewModel } from '../../../../../assets/models/farm.read.viewmodel';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-farms.view',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    RouterLink
  ],
  templateUrl: './farms.view.component.html',
  styleUrl: './farms.view.component.scss'
})
export class FarmsViewComponent implements OnInit {
  private farmService = inject(FarmService);
  private router = inject(Router);

  farms: FarmReadViewModel[] = [];

  ngOnInit(): void {
    this.farmService.getMy()
      .subscribe(result => {
        this.farms = result;
      });
  }

  onFarmDetails(farmId: number){
    this.router.navigate(['farms', farmId]);
  }
}
