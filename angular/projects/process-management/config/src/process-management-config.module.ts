import { ModuleWithProviders, NgModule } from '@angular/core';
import { PROCESS_MANAGEMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class ProcessManagementConfigModule {
  static forRoot(): ModuleWithProviders<ProcessManagementConfigModule> {
    return {
      ngModule: ProcessManagementConfigModule,
      providers: [PROCESS_MANAGEMENT_ROUTE_PROVIDERS],
    };
  }
}
