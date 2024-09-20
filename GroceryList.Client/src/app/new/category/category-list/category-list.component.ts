import { Component, inject, OnInit } from '@angular/core';
import { Category } from '../types/category.type';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { ProductState } from '../ngxs-store/product.state';
import { LetDirective } from '@ngrx/component';
import { GetCategories } from '../ngxs-store/product.action';
import { Router } from '@angular/router';
import { ROUTES_PARAM } from '../../../constants';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, LetDirective],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent {
  #ngxsStore = inject(Store);
  #router = inject(Router);
  categories$: Observable<Category[]> = this.#ngxsStore.select(ProductState.getCategories);

  onSelectCategory = (categoryId: string) => {
    console.log('CategoryListComponent -> onSelectCategory -> categoryId', categoryId);
    this.#router.navigate([`/${ROUTES_PARAM.CATEGORY.CATEGORY}`, categoryId]);
  }
}
