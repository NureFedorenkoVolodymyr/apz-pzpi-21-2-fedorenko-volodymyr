import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TurbinesViewComponent } from './turbines.view.component';

describe('TurbinesViewComponent', () => {
  let component: TurbinesViewComponent;
  let fixture: ComponentFixture<TurbinesViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TurbinesViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TurbinesViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
