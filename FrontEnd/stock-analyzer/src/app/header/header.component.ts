import { Component } from '@angular/core';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  enableHeader: boolean = false;

  constructor() {
    fromEvent(window, 'scroll').subscribe((event) => {
      window.pageYOffset >= 35
        ? (this.enableHeader = true)
        : (this.enableHeader = false);
    });
  }

}
