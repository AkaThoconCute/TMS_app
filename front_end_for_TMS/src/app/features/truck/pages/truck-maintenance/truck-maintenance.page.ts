import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-truck-maintenance',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-6">
      <h1 class="text-2xl font-bold mb-4">Truck Maintenance</h1>
      <p class="text-gray-600">Quản lý bảo trì và sửa chữa xe.</p>
    </div>
  `
})
export class TruckMaintenancePage { }