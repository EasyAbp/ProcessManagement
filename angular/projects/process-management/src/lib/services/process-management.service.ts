import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class ProcessManagementService {
  apiName = 'ProcessManagement';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/ProcessManagement/sample' },
      { apiName: this.apiName }
    );
  }
}
