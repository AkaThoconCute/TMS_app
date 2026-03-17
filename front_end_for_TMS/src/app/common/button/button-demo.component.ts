import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'button-demo',
  templateUrl: `button-demo.component.html`,
  imports: [ButtonModule]
})
export class ButtonDemo {
  toggleDarkMode() {
    // const element = document.querySelector('html');
    // element.classList.toggle('my-app-dark');
  }
}