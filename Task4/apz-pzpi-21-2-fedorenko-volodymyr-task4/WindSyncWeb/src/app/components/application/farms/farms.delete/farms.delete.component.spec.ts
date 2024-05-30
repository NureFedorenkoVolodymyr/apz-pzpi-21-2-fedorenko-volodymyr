import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmsDeleteComponent } from './farms.delete.component';

describe('FarmsDeleteComponent', () => {
  let component: FarmsDeleteComponent;
  let fixture: ComponentFixture<FarmsDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmsDeleteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FarmsDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
