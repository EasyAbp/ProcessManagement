import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ProcessManagementComponent } from './process-management.component';

describe('ProcessManagementComponent', () => {
  let component: ProcessManagementComponent;
  let fixture: ComponentFixture<ProcessManagementComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
