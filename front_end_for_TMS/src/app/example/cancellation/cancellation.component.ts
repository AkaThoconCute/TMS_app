import { Component, OnDestroy } from '@angular/core';
import { CancellationService } from '@app/example/cancellation/cancellation.service';
import { ButtonModule } from 'primeng/button';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-cancellation',
  imports: [ButtonModule],
  templateUrl: './cancellation.component.html',
  styleUrl: './cancellation.component.css',
})
export class CancellationComponent implements OnDestroy {
  
  private activeRequestSub?: Subscription;

  constructor(
    private cancellationService: CancellationService
  ) { }

  makeWorkRequest(): void {
    // Logic to make a work request
    console.log('Making a work request...');

    // Clean up any existing running request before starting a new one
    this.cancelWorkRequest();

    this.activeRequestSub = this.cancellationService.makeRequest().subscribe({
      next: () => {
        console.log('Work request made successfully.');
        this.activeRequestSub = undefined;
      },
      error: (error) => {
        console.error('Error making work request:', error);
        this.activeRequestSub = undefined;
      }
    });
  }

  cancelWorkRequest(): void {
    if (this.activeRequestSub) {
      console.log('Aborting HTTP connection...');
      this.activeRequestSub.unsubscribe(); // Aborts the HTTP request -> ASP.NET Core gets CancellationToken signal
      this.activeRequestSub = undefined;
    }
  }

  ngOnDestroy(): void {
    this.cancelWorkRequest();
  }
}
