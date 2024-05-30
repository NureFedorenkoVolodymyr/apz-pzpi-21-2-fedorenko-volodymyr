import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TurbinesAddComponent } from './turbines.add.component';

describe('TurbinesAddComponent', () => {
  let component: TurbinesAddComponent;
  let fixture: ComponentFixture<TurbinesAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TurbinesAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TurbinesAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
