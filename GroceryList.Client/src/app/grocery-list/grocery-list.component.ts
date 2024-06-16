import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-grocery-list',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './grocery-list.component.html',
  styleUrl: './grocery-list.component.scss'
})
export class GroceryListComponent {
  
}
