import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideStore } from '@ngxs/store';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { GroceryListDetailsComponent } from './grocery-list-details.component';
import { StoreState } from '../../store/ngxs-store/store.state';
import { IngredientState } from '../ngxs-store/ingredient.state';
import { GroceryListState } from '../ngxs-store/grocery-list.state';

describe('GroceryListDetailsComponent', () => {
  let component: GroceryListDetailsComponent;
  let fixture: ComponentFixture<GroceryListDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GroceryListDetailsComponent],
      providers: [provideRouter([]), provideStore([StoreState, IngredientState, GroceryListState]), provideHttpClient(), provideHttpClientTesting()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroceryListDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
