import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FarmService } from '../../../../services/farm.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FarmAddViewModel } from '../../../../../assets/models/farm.add.viewmodel';
import { FarmReadViewModel } from '../../../../../assets/models/farm.read.viewmodel';

@Component({
  selector: 'app-farms.add',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './farms.add.component.html',
  styleUrl: './farms.add.component.scss'
})
export class FarmsAddComponent implements OnInit {
  private fb = inject(FormBuilder);
  private farmService = inject(FarmService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  farmForm = this.fb.group({
    address: ['', [Validators.required]]
  });

  farmId?: number;
  farmUserId?: string;
  formActionLabel: string = 'Add';

  ngOnInit(): void {
    this.farmId = this.route.snapshot.params['id'] as number;
    if(this.farmId){
      this.formActionLabel = 'Update';

      this.farmService.getById(this.farmId)
        .subscribe(result => {
          this.farmUserId = result.userId;
          this.farmForm.patchValue(result);
        });
    }
  }

  onSubmit(){
    if(!this.farmForm.valid)
      return;

    let farm = this.farmForm.getRawValue() as FarmReadViewModel;

    if(this.farmId && this.farmUserId){
      farm.id = this.farmId;
      farm.userId = this.farmUserId;
      this.farmService.update(farm)
        .subscribe(() => {this.navigateToMain()});
      return;
    }

    this.farmService.add(farm)
      .subscribe(() => {this.navigateToMain()});
  }

  navigateToMain(){
    if(this.farmId)
      this.router.navigate(['farms', this.farmId]);
    else
      this.router.navigate(['farms']);
  }
}
