import { TestBed } from '@angular/core/testing';

import { ProcessManagementService } from './process-management.service';

describe('ProcessManagementService', () => {
  let service: ProcessManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcessManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
