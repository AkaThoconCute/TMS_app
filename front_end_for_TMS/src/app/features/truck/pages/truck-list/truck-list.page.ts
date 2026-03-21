import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { TruckService } from '@features/truck/services/truck.service';
import { TruckDto } from '@features/truck/models/truck.models';

const STATUS_MAP: Record<number, { label: string; severity: 'success' | 'info' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Available', severity: 'success' },
  2: { label: 'In Use', severity: 'info' },
  3: { label: 'Maintenance', severity: 'warn' },
  4: { label: 'Broken Down', severity: 'danger' },
  5: { label: 'Retired', severity: 'secondary' },
};

const OWNERSHIP_MAP: Record<number, string> = {
  1: 'Owned',
  2: 'Leased',
};

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    TagModule,
    TooltipModule,
  ],
  templateUrl: './truck-list.page.html',
  styleUrl: './truck-list.page.css',
})
export class TruckListPage {
  private readonly truckService = inject(TruckService);

  trucks = signal<TruckDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(3);
  first = signal(0);

  readonly rowsPerPageOptions = [2, 3, 4];

  loadTrucks(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const page = Math.floor((event.first ?? 0) / (event.rows ?? this.rows())) + 1;
    const size = event.rows ?? this.rows();

    this.truckService.list(page, size).subscribe({
      next: (res) => {
        this.trucks.set(res.data);
        this.totalRecords.set(res.totalCount);
        this.first.set(event.first ?? 0);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  getStatusLabel(status: number): string {
    return STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'success' | 'info' | 'warn' | 'danger' | 'secondary' {
    return STATUS_MAP[status]?.severity ?? 'secondary';
  }

  getOwnershipLabel(type: number): string {
    return OWNERSHIP_MAP[type] ?? 'Unknown';
  }

  onAdd(): void {
    // no-logic placeholder
  }

  onEdit(_truck: TruckDto): void {
    // no-logic placeholder
  }

  onDelete(_truck: TruckDto): void {
    // no-logic placeholder
  }
}
