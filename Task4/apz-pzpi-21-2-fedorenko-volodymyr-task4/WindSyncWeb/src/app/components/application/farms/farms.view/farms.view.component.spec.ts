import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmsViewComponent } from './farms.view.component';

describe('FarmsViewComponent', () => {
  let component: FarmsViewComponent;
  let fixture: ComponentFixture<FarmsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmsViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FarmsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
