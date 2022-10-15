import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ProcessManagementComponent } from './components/process-management.component';
import { ProcessManagementRoutingModule } from './process-management-routing.module';

@NgModule({
  declarations: [ProcessManagementComponent],
  imports: [CoreModule, ThemeSharedModule, ProcessManagementRoutingModule],
  exports: [ProcessManagementComponent],
})
export class ProcessManagementModule {
  static forChild(): ModuleWithProviders<ProcessManagementModule> {
    return {
      ngModule: ProcessManagementModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<ProcessManagementModule> {
    return new LazyModuleFactory(ProcessManagementModule.forChild());
  }
}
