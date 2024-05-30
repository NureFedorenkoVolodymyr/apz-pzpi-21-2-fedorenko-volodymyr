import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TurbinesDetailsComponent } from './turbines.details.component';

describe('TurbinesDetailsComponent', () => {
  let component: TurbinesDetailsComponent;
  let fixture: ComponentFixture<TurbinesDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TurbinesDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TurbinesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
