import { Component, computed, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@platform/auth/auth.service';

interface MenuItem {
  label: string;
  route: string;
}

interface MenuGroup {
  label: string;
  icon: string;
  adminOnly?: boolean;
  items: MenuItem[];
}

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
  isAdmin = computed(() => this.authService.hasRole('Admin'));

  menuGroups: MenuGroup[] = [
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
      label: 'Customer',
      icon: 'groups',
      items: [
        { label: 'List customer', route: '/customers/list' }
      ]
    },
    {
      label: 'Order',
      icon: 'receipt_long',
      items: [
        { label: 'List order', route: '/orders/list' }
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
    },
    {
      label: 'Admin',
      icon: 'admin_panel_settings',
      adminOnly: true,
      items: [
        { label: 'Users', route: '/admin/users' }
      ]
    }
  ];
}