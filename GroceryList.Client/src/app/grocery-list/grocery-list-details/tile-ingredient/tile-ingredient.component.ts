import { Component, EventEmitter, Input, Output, input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ButtonComponent } from '../../../shared/button/button.component';
import { Ingredient } from '../../types/ingredient.type';
import { ButtonStyle } from '../../../shared/button/button-style.enum';
import { ButtonHover } from '../../../shared/button/button-hover.enum';

@Component({
  selector: 'app-tile-ingredient',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './tile-ingredient.component.html',
  styleUrl: './tile-ingredient.component.scss'
})
export class TileIngredientComponent {
  ingredient = input.required<Ingredient>();
  @Output() onIngredientDeleted: EventEmitter<string> = new EventEmitter<string>();

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  public get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  showDeleteList = (event: Event) => {
    event.stopImmediatePropagation();
    this.onIngredientDeleted.emit(this.ingredient().id);
  }
}
