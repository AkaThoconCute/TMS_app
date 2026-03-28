import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-driver-salary',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-6">
      <h1 class="text-2xl font-bold mb-4">Driver Salary</h1>
      <p class="text-gray-600">Salary management will be available in a future update.</p>
    </div>
  `
})
export class DriverSalaryPage { }
