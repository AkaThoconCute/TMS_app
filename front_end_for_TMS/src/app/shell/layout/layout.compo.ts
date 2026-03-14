import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { SideNavCompo } from '../side-nav/side-nav.compo';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, RouterLinkActive, SideNavCompo],
  templateUrl: './layout.compo.html',
  styleUrl: './layout.compo.css'
})
export class LayoutComponent {
  exactMatch = { exact: true };

  isSidebarOpen = signal(true);

  toggleSidebar() {
    this.isSidebarOpen.update(v => !v);
  }
}
