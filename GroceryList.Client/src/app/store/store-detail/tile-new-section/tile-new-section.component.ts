import { Component, ElementRef, EventEmitter, HostListener, OnInit, Output, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { ButtonComponent } from '../../../shared/button/button.component';
import { ButtonHover } from '../../../shared/button/button-hover.enum';
import { Section } from '../../types/section.type';
import { SECTION_FORM } from '../../../constants';

@Component({
  selector: 'app-tile-new-section',
  standalone: true,
  imports: [CommonModule, ButtonComponent, ReactiveFormsModule],
  templateUrl: './tile-new-section.component.html',
  styleUrl: './tile-new-section.component.scss'
})
export class TileNewSectionComponent implements OnInit {
  @Output() itemAdded: EventEmitter<Section> = new EventEmitter<Section>();
  @Output() onClickOutside: EventEmitter<void> = new EventEmitter<void>();
  elementRef = inject(ElementRef)
  addForm!: FormGroup;
  formSubmitted: boolean = false;
  @ViewChild('inputField') inputField!: ElementRef;

  get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  @HostListener('document:click', ['$event.target'])
  onClick(target: EventTarget | null): void {
    const clickedInside = this.elementRef.nativeElement.contains(target);
    if (!clickedInside) {
      this.onClickOutside.emit();
    }
  }

  ngOnInit(): void {
    this.#initForm();
  }

  ngAfterViewInit(): void {
    this.inputField.nativeElement.focus();
  }

  addItem = () => {
    this.formSubmitted = true;
    if (this.addForm.invalid) return;
    this.itemAdded.emit(this.addForm.value);
    this.formSubmitted = false;
  }

  onEnterPressed = () => {
    this.addItem();
  }

  #initForm = () => {
    this.addForm = new FormGroup({
      [SECTION_FORM.NAME]: new FormControl('', Validators.required),
    });
  }
}
