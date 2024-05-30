import { Component, inject } from '@angular/core';
import { TurbineService } from '../../../../services/turbine.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { TurbineAddViewModel } from '../../../../../assets/models/turbine.add.viewmodel';
import { TurbineReadViewModel } from '../../../../../assets/models/turbine.read.viewmodel';

@Component({
  selector: 'app-turbines.add',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './turbines.add.component.html',
  styleUrl: './turbines.add.component.scss'
})
export class TurbinesAddComponent {
  private fb = inject(FormBuilder);
  private turbineService = inject(TurbineService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  turbineForm!: FormGroup;

  farmId!: number;
  turbineId?: number;
  formActionLabel: string = 'Add';

  ngOnInit(): void {
    this.farmId = this.route.snapshot.params['farm-id'] as number;
    this.turbineId = this.route.snapshot.params['id'] as number;

    this.turbineForm = this.fb.group({
      turbineRadius: [1, [Validators.required, Validators.min(1)]],
      latitude: [0, [Validators.required]],
      longitude: [0, [Validators.required]],
      altitude: [0, [Validators.required]],
      efficiency: [50, [Validators.required, Validators.min(0), Validators.max(100)]],
      cutInWindSpeed: [0, [Validators.required]],
      ratedWindSpeed: [0, [Validators.required]],
      shutDownWindSpeed: [0, [Validators.required]],
      windFarmId: [this.farmId, [Validators.required]]
    });

    if(this.turbineId){
      this.formActionLabel = 'Update';

      this.turbineService.getById(this.turbineId)
        .subscribe(result => {
          this.turbineForm.patchValue(result);
        });
    }
  }

  onSubmit(){
    if(!this.turbineForm.valid)
      return;

    let turbine = this.turbineForm.getRawValue() as TurbineAddViewModel;

    if(this.turbineId){
      this.turbineService.update(this.turbineId, turbine)
        .subscribe(() => {this.navigateToMain()});
      return;
    }
    
    this.turbineService.add(turbine)
        .subscribe(() => {this.navigateToMain()});
  }

  navigateToMain(){
    this.router.navigate(['farms', this.farmId]);
  }
}
