import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FarmService } from '../../../../services/farm.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FarmAddViewModel } from '../../../../../assets/models/farm.add.viewmodel';

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
  formActionLabel: string = 'Add';
  cancelRouterLink: string = '../';

  ngOnInit(): void {
    this.farmId = this.route.snapshot.params['id'] as number;
    if(this.farmId){
      this.formActionLabel = 'Edit';
      this.cancelRouterLink = '../../';

      // this.farmForm.getById(this.employeeId)
      //   .subscribe(result => {
      //     this.employeeForm.patchValue(result);
      //     this.employeeForm.controls.email.disable();
      //   });
    }
  }

  onSubmit(){
    if(!this.farmForm.valid)
      return;

    let farm = this.farmForm.getRawValue() as FarmAddViewModel;

    if(this.farmId){
      // employee.id = this.employeeId;
      // this.employeeService.update(employee)
      //   .subscribe(() => {this.navigateToMain()});
      // return;
    }

    this.farmService.add(farm)
      .subscribe(() => {this.navigateToMain()});
  }

  navigateToMain(){
    this.router.navigate(['farms']);
  }
}
