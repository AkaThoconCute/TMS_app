import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="p-6">
      <h1 class="text-2xl font-bold mb-4">Truck Management</h1>
      <p class="text-gray-600">Danh sách phương tiện vận tải.</p>
    </div>
  `
})
export class TruckListPage { }