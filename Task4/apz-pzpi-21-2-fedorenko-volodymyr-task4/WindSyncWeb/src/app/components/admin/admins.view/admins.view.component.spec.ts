import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminsViewComponent } from './admins.view.component';

describe('AdminsViewComponent', () => {
  let component: AdminsViewComponent;
  let fixture: ComponentFixture<AdminsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminsViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
