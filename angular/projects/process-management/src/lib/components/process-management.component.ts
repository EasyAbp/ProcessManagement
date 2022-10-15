import { Component, OnInit } from '@angular/core';
import { ProcessManagementService } from '../services/process-management.service';

@Component({
  selector: 'lib-process-management',
  template: ` <p>process-management works!</p> `,
  styles: [],
})
export class ProcessManagementComponent implements OnInit {
  constructor(private service: ProcessManagementService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
