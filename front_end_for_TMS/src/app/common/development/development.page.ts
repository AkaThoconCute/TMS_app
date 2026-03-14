import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-development',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './development.page.html',
  styleUrl: './development.page.css'
})
export class DevelopmentPage {
  supportAccount = {
    bank: 'Vietcombank',
    number: '1234567890',
    owner: 'NGUYEN TRUNG NHAN'
  };

  copyAccount() {
    navigator.clipboard.writeText(this.supportAccount.number);
    // You can add a snackbar notification here later
  }
}