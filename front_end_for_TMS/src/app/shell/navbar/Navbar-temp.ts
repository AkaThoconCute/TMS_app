import { Component, computed, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@platform/auth/auth.service';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
})
export class Navbar {
  private authService = inject(AuthService);
  private currentUser = toSignal(this.authService.currentUser$);

  isOpen = input<boolean>(true);
  toggleNav = output<void>();
  tenantName = computed(() => this.currentUser()?.tenantName || 'My Business');

  menuGroups = [
    {
      label: 'Truck',
      icon: 'local_shipping',
      items: [
        { label: 'List truck', route: '/trucks/list' },
        { label: 'Maintain truck', route: '/trucks/maintenance' }
      ]
    },
    {
      label: 'Driver',
      icon: 'person',
      items: [
        { label: 'List driver', route: '/drivers/list' },
        { label: 'Calculate salary', route: '/drivers/salary' }
      ]
    },
    {
      label: 'Demo',
      icon: 'person',
      items: [
        { label: 'Button', route: '/demo/button' },
        { label: 'Upcoming', route: '/demo/upcoming' }
      ]
    },
    {
      label: 'Account',
      icon: 'account_circle',
      items: [
        { label: 'My Profile', route: '/profile' }
      ]
    }
  ];
}