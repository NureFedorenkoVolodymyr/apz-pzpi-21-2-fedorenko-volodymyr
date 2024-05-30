import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmsAddComponent } from './farms.add.component';

describe('FarmsAddComponent', () => {
  let component: FarmsAddComponent;
  let fixture: ComponentFixture<FarmsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmsAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FarmsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
