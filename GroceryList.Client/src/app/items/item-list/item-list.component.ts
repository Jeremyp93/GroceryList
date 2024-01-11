import { Component, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription, take } from 'rxjs';

import { Item } from '../types/item';
import { ItemsService } from '../items.service';
import { ButtonComponent } from '../../shared/button/button.component';
import { ButtonStyle } from '../../shared/button/button-style.enum';
import { InputFieldComponent } from '../../shared/input-field/input-field.component';
import { ConfirmDialogService } from '../../shared/confirm-dialog/confirm-dialog.service';
import { ButtonHover } from '../../shared/button/button-hover.enum';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonComponent, InputFieldComponent],
  templateUrl: './item-list.component.html',
  styleUrl: './item-list.component.scss'
})
export class ItemListComponent implements OnInit, OnDestroy {
  itemsService = inject(ItemsService);
  #confirmDialogService = inject(ConfirmDialogService);
  searchText: string = '';
  items: Item[] = [];
  filteredItems: Item[] = [];
  addingProcess: boolean = false;
  @ViewChild('newItemNameInput', {static: false}) inputFieldComponent!: InputFieldComponent;
  #itemSubscription!: Subscription;

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  get buttonHoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  async ngOnInit(): Promise<void> {
    this.itemsService.getAllItems();
    this.#itemSubscription = this.itemsService.items$.subscribe(items => {
      this.items = items;
      this.filteredItems = items;
    });
  }

  onInputChange = (event: Event): void => {
    this.filteredItems = this.items.filter(i => i.name.toLowerCase().startsWith(this.searchText.toLowerCase()));
  }

  deleteItem = (itemId: string) => {
    this.#confirmDialogService.setQuestion(`Are you sure you want to delete item ${this.filteredItems.find(i => i.id === itemId)?.name} ?`);
    this.#confirmDialogService.answer$.pipe(take(1)).subscribe(response => {
      if (response) {
        this.itemsService.deleteItem(itemId);
        this.searchText = '';
      }
    })
  }

  startAddingProcess = () => {
    this.addingProcess = true;
    setTimeout(() => {
      this.inputFieldComponent.focusInput();
    }, 100);
    
  }

  stopAddingProcess = () => {
    this.addingProcess = false;
  }

  addItem = (name: string) => {
    this.itemsService.addItem(name);
    this.stopAddingProcess();
  }

  ngOnDestroy(): void {
    this.#itemSubscription.unsubscribe();
  }
}
