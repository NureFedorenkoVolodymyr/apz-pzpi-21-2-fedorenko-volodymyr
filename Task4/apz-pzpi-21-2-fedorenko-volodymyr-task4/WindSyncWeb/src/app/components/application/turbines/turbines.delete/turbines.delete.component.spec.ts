import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TurbinesDeleteComponent } from './turbines.delete.component';

describe('TurbinesDeleteComponent', () => {
  let component: TurbinesDeleteComponent;
  let fixture: ComponentFixture<TurbinesDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TurbinesDeleteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TurbinesDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
