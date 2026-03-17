import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './side-nav.compo.html',
  // styleUrl: './side-nav.compo.css'
})
export class SideNavCompo {
  isOpen = input<boolean>(true);
  toggleNav = output<void>();

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
        { label: 'List driver', route: '/drivers' },
        { label: 'Calculate salary', route: '/drivers/salary' }
      ]
    },
    {
      label: 'Demo',
      icon: 'person',
      items: [
        { label: 'Button', route: '/button-demo' },
        { label: 'Upcoming', route: '/upcoming' }
      ]
    }
  ];
}