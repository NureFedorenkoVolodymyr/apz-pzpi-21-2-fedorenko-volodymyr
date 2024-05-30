import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmsDetailsComponent } from './farms.details.component';

describe('FarmsDetailsComponent', () => {
  let component: FarmsDetailsComponent;
  let fixture: ComponentFixture<FarmsDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmsDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FarmsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
