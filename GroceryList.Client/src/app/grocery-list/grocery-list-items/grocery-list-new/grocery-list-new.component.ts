import { Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChildren, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, Subscription, lastValueFrom } from 'rxjs';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { v4 as UUID } from 'uuid';
import { Store as NgxsStore, Select } from '@ngxs/store';
import { CdkDropListGroup, CdkDropList, CdkDrag, CdkDragHandle, CdkDragDrop } from '@angular/cdk/drag-drop';

import { HeaderComponent } from '../../../shared/header/header.component';
import { StoreService } from '../../../store/store.service';
import { ButtonComponent } from '../../../shared/button/button.component';
import { Ingredient } from '../../types/ingredient.type';
import { Store } from '../../../store/types/store.type';
import { AddGroceryList, GetSelectedGroceryList, SetSelectedGroceryList, UpdateGroceryList } from '../../ngxs-store/grocery-list.actions';
import { ButtonStyle } from '../../../shared/button/button-style.enum';
import { ROUTES_PARAM, GROCERY_LIST_FORM, INGREDIENT_FORM } from '../../../constants';
import { GroceryListState } from '../../ngxs-store/grocery-list.state';
import { GroceryList } from '../../types/grocery-list.type';
import { LoadingComponent } from '../../../shared/loading/loading.component';
import { LoadingSize } from '../../../shared/loading/loading-size.enum';
import { LoadingColor } from '../../../shared/loading/loading-color.enum';
import { ButtonHover } from '../../../shared/button/button-hover.enum';
import { GetStores } from '../../../store/ngxs-store/store.actions';
import { StoreState } from '../../../store/ngxs-store/store.state';
import { InputFieldComponent } from '../../../shared/input-field/input-field.component';
import { InputType } from '../../../shared/input-field/input-type.enum';
import { ItemsService } from '../../../items/items.service';
import { Item } from '../../../items/types/item';

@Component({
  selector: 'app-grocery-list-new',
  standalone: true,
  imports: [CommonModule, HeaderComponent, ButtonComponent, ReactiveFormsModule, LoadingComponent, InputFieldComponent, CdkDropListGroup, CdkDropList, CdkDrag, CdkDragHandle],
  templateUrl: './grocery-list-new.component.html',
  styleUrl: './grocery-list-new.component.scss'
})
export class GroceryListNewComponent implements OnInit, OnDestroy {
  #storeService = inject(StoreService);
  #router = inject(Router);
  #route = inject(ActivatedRoute);
  #ngStore = inject(NgxsStore);
  #itemsService = inject(ItemsService);
  @Select(StoreState.getStores) stores$!: Observable<Store[]>;
  groceryListForm!: FormGroup;
  categories: any[] = [];
  editMode: boolean = false;
  idToEdit: string | null = null;
  submitted: boolean = false;
  isLoading: boolean = false;
  title: string = '';
  items$!: Observable<Item[]>;
  #routeSubscription!: Subscription;

  @ViewChildren('inputFields', { read: InputFieldComponent }) inputFields!: QueryList<InputFieldComponent>;

  get ingredientControls() { // a getter!
    return (this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray).controls;
  }

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  get loadingSizes(): typeof LoadingSize {
    return LoadingSize;
  }

  get loadingColors(): typeof LoadingColor {
    return LoadingColor;
  }

  get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  get inputTypes(): typeof InputType {
    return InputType;
  }
  ngOnInit(): void {
    this.#routeSubscription = this.#route.params.subscribe(async (params: Params) => {
      this.idToEdit = params[ROUTES_PARAM.ID_PARAMETER];
      if (this.idToEdit) {
        this.editMode = true;
      }
      this.title = `${this.editMode ? 'Edit' : 'Add'} Grocery List`;
      this.#ngStore.dispatch(new GetStores());
      await this.#initForm();
      this.items$ = this.#itemsService.items$;
    });
  }

  onAddIngredient = (category: string) => {
    const id = UUID();
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    ingredients.insert(0, new FormGroup({
      [INGREDIENT_FORM.ID]: new FormControl(id),
      [INGREDIENT_FORM.NAME]: new FormControl(null, Validators.required),
      [INGREDIENT_FORM.AMOUNT]: new FormControl("1", [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
      [INGREDIENT_FORM.CATEGORY]: new FormControl(category)
    }));
    setTimeout(() => {
      this.#focusOnControl(id);
    }, 100);
  }

  onDeleteIngredient = (id: string) => {
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    const index = ingredients.controls.findIndex(c => c.get(INGREDIENT_FORM.ID)?.value === id);
    ingredients.removeAt(index);
    this.groceryListForm.markAsDirty();
  }

  onSubmit = () => {
    console.log(this.groceryListForm.value);
    this.submitted = true;
    if (this.groceryListForm.invalid) return;
    this.isLoading = true;
    if (this.editMode) {
      if (!this.groceryListForm.pristine) {
        this.#ngStore.dispatch(new UpdateGroceryList(this.groceryListForm.value, this.idToEdit!)).subscribe({
          next: () => {
            const updatedList = this.#ngStore.selectSnapshot(GroceryListState.getLastUpdatedGroceryList);
            this.#ngStore.dispatch(new SetSelectedGroceryList(updatedList)).subscribe(() => this.#back());
          },
          error: () => this.isLoading = false
        });
      } else {
        this.#back();
      }
    } else {
      this.#ngStore.dispatch(new AddGroceryList(this.groceryListForm.value)).subscribe({
        next: () => this.#back(),
        error: () => this.isLoading = false
      });
    }
  }

  onChangeStore = async (event: Event) => {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.#resetCategory();
    if (!selectedValue) {
      this.categories = [];
      return;
    }
    const store = await lastValueFrom(this.#storeService.getStoreById(selectedValue));
    this.categories = [...store.sections.map(s => s.name), ''];
  }

  onEnterPressed = (event: KeyboardEvent | Event, category: string) => {
    event.preventDefault();
    this.onAddIngredient(category);
  }

  getIngredientsControl = (cateogry: string) => {
    if (!cateogry) {
      return (this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray).controls.filter(c => !c.value.category);
    }
    return (this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray).controls.filter(c => c.value.category === cateogry);
  }

  getIndex(ingredientId: string): number {
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    return ingredients.controls.findIndex(i => i.value.id === ingredientId);
  }

  drop = (event: CdkDragDrop<string>) => {
    const id = UUID();
    const item = event.item.element.nativeElement.innerText;
    const category = event.container.data;
    if (this.#ingredientExist(item, category)) {
      return;
    }
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    ingredients.insert(0, new FormGroup({
      [INGREDIENT_FORM.ID]: new FormControl(id),
      [INGREDIENT_FORM.NAME]: new FormControl(item, Validators.required),
      [INGREDIENT_FORM.AMOUNT]: new FormControl("1", [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
      [INGREDIENT_FORM.CATEGORY]: new FormControl(category)
    }));
  }

  ngOnDestroy(): void {
    this.#routeSubscription.unsubscribe();
  }

  #initForm = async () => {
    if (this.editMode) {
      const groceryList = this.#ngStore.selectSnapshot(GroceryListState.getSelectedGroceryList);
      if (!groceryList) {
        await lastValueFrom(this.#ngStore.dispatch(new GetSelectedGroceryList(this.idToEdit!)));
        const list = this.#ngStore.selectSnapshot(GroceryListState.getSelectedGroceryList);
        this.#setupEditForm(list!);
        return;
      }

      this.#setupEditForm(groceryList!);
    } else {
      this.#setupForm('', '', new FormArray<any>([]));
    }
  }

  #setupEditForm = (groceryList: GroceryList) => {
    const name = groceryList.name;
    let storeId = '';
    let ingredients: FormArray<any> = new FormArray<any>([]);
    if (groceryList.store) {
      storeId = groceryList.store.id;
      this.categories = [...groceryList.store.sections.map(s => s.name), ''];
    }
    if (groceryList.ingredients.length > 0) {
      console.log(groceryList.ingredients);
      groceryList.ingredients.forEach((ingredient: Ingredient) => {
        ingredients.push(new FormGroup({
          [INGREDIENT_FORM.ID]: new FormControl(UUID()),
          [INGREDIENT_FORM.NAME]: new FormControl(ingredient.name, Validators.required),
          [INGREDIENT_FORM.AMOUNT]: new FormControl(ingredient.amount, [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
          [INGREDIENT_FORM.CATEGORY]: new FormControl(this.categories.includes(ingredient.category) ? ingredient.category : '')
        }));
      });
    }

    this.#setupForm(name, storeId, ingredients);
  }

  #setupForm = (name: string, storeId: string, ingredients: FormArray) => {
    this.groceryListForm = new FormGroup({
      [GROCERY_LIST_FORM.NAME]: new FormControl(name, Validators.required),
      [GROCERY_LIST_FORM.STORE_ID]: new FormControl(storeId),
      [GROCERY_LIST_FORM.INGREDIENTS]: ingredients,
    });
  }

  #focusOnControl = (id: string) => {
    const elements = this.inputFields.toArray();
    console.log(elements);
    const element = elements.find(e => e.id === `name_${id}`);
    if (element) {
      element.focusInput();
    }
  }

  #back = () => {
    this.#router.navigate(['../'], { relativeTo: this.#route });
  }

  #resetCategory = () => {
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    for (let i = 0; i < ingredients.length; i++) {
      const formGroup = ingredients.at(i) as FormGroup;
      formGroup.get(INGREDIENT_FORM.CATEGORY)?.setValue('');
    }
  }

  #ingredientExist = (item: string, category: string): boolean => {
    const ingredients = this.groceryListForm.get(GROCERY_LIST_FORM.INGREDIENTS) as FormArray;
    return ingredients.controls.find(c => c.value.name === item && c.value.category === category) ? true : false;
  }
}
