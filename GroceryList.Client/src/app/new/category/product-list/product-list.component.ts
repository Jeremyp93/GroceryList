import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { lastValueFrom, Observable, Subscription } from 'rxjs';
import { Product } from '../types/Product.type';
import { ProductState } from '../ngxs-store/product.state';
import { ActivatedRoute, Params } from '@angular/router';
import { ROUTES_PARAM } from '../../../constants';
import { GetProductsByCategory } from '../ngxs-store/product.action';
import { CommonModule } from '@angular/common';
import { LetDirective } from '@ngrx/component';
import { LoadingComponent } from '../../../shared/loading/loading.component';
import { LoadingSize } from '../../../shared/loading/loading-size.enum';
import { LoadingColor } from '../../../shared/loading/loading-color.enum';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, LetDirective, LoadingComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit, OnDestroy {
  #ngxsStore = inject(Store);
  #route = inject(ActivatedRoute);
  #routeSubscription!: Subscription;
  categoryId: string = '';
  products$: Observable<Product[]> = this.#ngxsStore.select(ProductState.getFilteredProducts);

  get loadingSizes(): typeof LoadingSize {
    return LoadingSize;
  }

  get loadingColors(): typeof LoadingColor {
    return LoadingColor;
  }

  ngOnInit(): void {
    this.#routeSubscription = this.#route.params.subscribe((params: Params) => {
      this.categoryId = params[ROUTES_PARAM.ID_PARAMETER];
      try {
        this.#ngxsStore.dispatch(new GetProductsByCategory(this.categoryId));
      } catch {
        return;
      }
    });
  }

  ngOnDestroy(): void {
    this.#routeSubscription.unsubscribe();
  }
}

