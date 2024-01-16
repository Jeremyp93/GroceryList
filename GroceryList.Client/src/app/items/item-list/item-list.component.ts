import { Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { take } from 'rxjs';

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
export class ItemListComponent implements OnInit {
  #itemsService = inject(ItemsService);
  #confirmDialogService = inject(ConfirmDialogService);
  searchText: string = '';
  addingProcess: boolean = false;
  @ViewChild('newItemNameInput', {static: false}) inputFieldComponent!: InputFieldComponent;
  items = computed(() => {
    const sq = this.#searchQuery();
    return this.#itemsService.items().filter(i => i.name.toLowerCase().startsWith(sq.toLowerCase()));
  });
  #searchQuery = signal<string>('');

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  get buttonHoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  async ngOnInit(): Promise<void> {
    this.#itemsService.getAllItems();
  }

  onInputChange = (event: Event): void => {
    this.#searchQuery.set(this.searchText.toLowerCase());
  }

  deleteItem = (itemId: string) => {
    this.#confirmDialogService.setQuestion(`Are you sure you want to delete item ${this.items().find(i => i.id === itemId)?.name} ?`);
    this.#confirmDialogService.answer$.pipe(take(1)).subscribe(response => {
      if (response) {
        this.#itemsService.deleteItem(itemId);
        this.searchText = '';
        this.#searchQuery.set('');
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
    this.#itemsService.addItem(name);
    this.stopAddingProcess();
  }
}
