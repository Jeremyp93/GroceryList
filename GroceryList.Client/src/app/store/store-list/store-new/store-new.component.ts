import { Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChildren, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { v4 as UUID } from 'uuid';

import { HeaderComponent } from '../../../shared/header/header.component';
import { ButtonComponent } from '../../../shared/button/button.component';
import { LoadingComponent } from '../../../shared/loading/loading.component';
import { ButtonHover } from '../../../shared/button/button-hover.enum';
import { ROUTES_PARAM, SECTION_FORM, STORE_FORM } from '../../../constants';
import { LoadingSize } from '../../../shared/loading/loading-size.enum';
import { LoadingColor } from '../../../shared/loading/loading-color.enum';
import { ButtonStyle } from '../../../shared/button/button-style.enum';
import { AlertService } from '../../../shared/alert/alert.service';
import { Alert, AlertType } from '../../../shared/alert/alert.model';

@Component({
  selector: 'app-store-new',
  standalone: true,
  imports: [CommonModule, HeaderComponent, ButtonComponent, ReactiveFormsModule, LoadingComponent],
  templateUrl: './store-new.component.html',
  styleUrl: './store-new.component.scss'
})
export class StoreNewComponent implements OnInit, OnDestroy {
  route = inject(ActivatedRoute);
  alertService = inject(AlertService);
  title: string = 'Add Store';
  storeForm!: FormGroup;
  isLoading: boolean = false;
  submitted: boolean = false;
  #routeSubscription!: Subscription;
  idToEdit: string | null = null;
  editMode: boolean = false;
  validationInProgress = false;

  @ViewChildren('inputFields') inputFields!: QueryList<ElementRef>;

  get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }
  get loadingSizes(): typeof LoadingSize {
    return LoadingSize;
  }
  get loadingColors(): typeof LoadingColor {
    return LoadingColor;
  }
  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  get sectionControls() { // a getter!
    return (this.storeForm.get(STORE_FORM.SECTIONS) as FormArray).controls;
  }

  ngOnInit(): void {
    this.#routeSubscription = this.route.params.subscribe(async (params: Params) => {
      this.idToEdit = params[ROUTES_PARAM.ID_PARAMETER];
      if (this.idToEdit) {
        this.editMode = true;
      }
      this.title = `${this.editMode ? 'Edit' : 'Add'} Store`;
      this.#initForm();
    });
  }

  onSubmit = () => {
    this.submitted = true;
    if (this.storeForm.invalid) return;
    if (this.#duplicatePrioritiesValidator()) {
      this.alertService.setAlertObs(new Alert(AlertType.Error, 'Duplicate priorities detected. Please correct the form.' ));
      return;
    }
    console.log(this.storeForm.value);
  }

  onAddSection = () => {
    const ingredients = this.storeForm.get(STORE_FORM.SECTIONS) as FormArray;
    ingredients.insert(0, new FormGroup({
      [SECTION_FORM.ID]: new FormControl(UUID()),
      [SECTION_FORM.NAME]: new FormControl(null, Validators.required),
      [SECTION_FORM.PRIORITY]: new FormControl(this.#getNextPriority(), [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
    }));
    setTimeout(() => {
      this.#focusOnControl(0);
    }, 100);
  }

  onEnterPressed = (event: KeyboardEvent | Event) => {
    event.preventDefault();
    this.onAddSection();
  }

  onDeleteSection = (id: string) => {
    const sections = this.storeForm.get(STORE_FORM.SECTIONS) as FormArray;
    const index = sections.controls.findIndex(c => c.get(SECTION_FORM.ID)?.value === id);
    sections.removeAt(index);
  }

  ngOnDestroy(): void {
    this.#routeSubscription.unsubscribe();
  }

  #initForm = () => {
    this.storeForm = new FormGroup({
      [STORE_FORM.NAME]: new FormControl('', Validators.required),
      [STORE_FORM.SECTIONS]: new FormArray<any>([]),
    });
  }

  #focusOnControl = (index: number) => {
    const elements = this.inputFields.toArray();
    if (elements[index]) {
      elements[index].nativeElement.focus();
    }
  }

  #getNextPriority = () => {
    const sections = this.storeForm.get(STORE_FORM.SECTIONS) as FormArray;
    const priorities = sections.controls.map((s) => s.get(SECTION_FORM.PRIORITY)?.value);
    if (priorities.length === 0) return 1;
    const nextPriority = +priorities[0]+1;
    if ((priorities as number[]).includes(nextPriority)) {
      return NaN;
    }
    return nextPriority;
  }

  #duplicatePrioritiesValidator = () => {
      const priorities = (this.storeForm.get(STORE_FORM.SECTIONS) as FormArray)?.controls.map((s) => s.get(SECTION_FORM.PRIORITY)?.value);
      return this.#hasDuplicates(priorities);
  }

  #hasDuplicates = (list: any[]) => {
    return new Set(list).size !== list.length;
  }
}
