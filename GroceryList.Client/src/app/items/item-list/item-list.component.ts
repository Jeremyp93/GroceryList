import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { lastValueFrom } from 'rxjs';

import { Item } from '../types/item';
import { ItemsService } from '../items.service';
import { ButtonComponent } from '../../shared/button/button.component';
import { ButtonStyle } from '../../shared/button/button-style.enum';
import { InputFieldComponent } from '../../shared/input-field/input-field.component';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonComponent, InputFieldComponent],
  templateUrl: './item-list.component.html',
  styleUrl: './item-list.component.scss'
})
export class ItemListComponent implements OnInit {
  itemsService = inject(ItemsService);
  searchText: string = '';
  filteredItems: Item[] = [];
  items: Item[] = [];

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  async ngOnInit(): Promise<void> {
    this.items = await lastValueFrom(this.itemsService.getAllItems());
    this.filteredItems = this.items;
  }

  onInputChange(event: Event): void {
    console.log(this.searchText);
    this.filteredItems = this.items.filter(i => i.name.toLowerCase().startsWith(this.searchText.toLowerCase()));
  }
}
