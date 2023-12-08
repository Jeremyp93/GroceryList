import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store as NgxsStore, Select } from '@ngxs/store';
import { StoreState } from '../ngxs-store/store.state';
import { Observable, Subscription, lastValueFrom, take } from 'rxjs';
import { Section } from '../types/section.type';
import { HeaderComponent } from '../../shared/header/header.component';
import { ActivatedRoute, Params } from '@angular/router';
import { ROUTES_PARAM } from '../../constants';
import { DropSection, GetSelectedStore } from '../ngxs-store/store.actions';
import { ButtonComponent } from '../../shared/button/button.component';
import { LoadingComponent } from '../../shared/loading/loading.component';
import { LoadingColor } from '../../shared/loading/loading-color.enum';
import { LoadingSize } from '../../shared/loading/loading-size.enum';
import { ButtonStyle } from '../../shared/button/button-style.enum';
import { CdkDropListGroup, CdkDropList,CdkDrag, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { LetDirective } from '@ngrx/component';

@Component({
  selector: 'app-store-detail',
  standalone: true,
  imports: [CommonModule, HeaderComponent, ButtonComponent, LoadingComponent, CdkDropListGroup, CdkDropList, CdkDrag, LetDirective],
  templateUrl: './store-detail.component.html',
  styleUrl: './store-detail.component.scss'
})
export class StoreDetailComponent implements OnInit, OnDestroy {
  private ngxsStore = inject(NgxsStore);
  private route = inject(ActivatedRoute);
  @Select(StoreState.getSections) sections$!: Observable<Section[]>;
  title: string = 'Sections';
  isLoading: boolean = false;
  id: string = '';
  #routeSubscription!: Subscription;
  saved: boolean = false;
  saveProcess: boolean = false;

  get loadingColors(): typeof LoadingColor {
    return LoadingColor;
  }

  get loadingSizes(): typeof LoadingSize {
    return LoadingSize;
  }

  get buttonStyles(): typeof ButtonStyle {
    return ButtonStyle;
  }

  ngOnInit(): void {
    this.isLoading = true;
    const selectedStore = this.ngxsStore.selectSnapshot(StoreState.getSelectedStore);
    if (selectedStore) {
      this.id = selectedStore.id;
      this.title = selectedStore.name;
      //this.initForm();
      this.isLoading = false;
      return;
    }

    this.#routeSubscription = this.route.params.subscribe(async (params: Params) => {
      this.id = params[ROUTES_PARAM.ID_PARAMETER];
      try {
        await lastValueFrom(this.ngxsStore.dispatch(new GetSelectedStore(this.id)));
      } catch {
        this.isLoading = false;
        return;
      }
      const selectedStore = this.ngxsStore.selectSnapshot(StoreState.getSelectedStore);
      if (!selectedStore) {
        this.isLoading = false;
        return;
      }
      this.title = selectedStore.name;
      //this.initForm();
      this.isLoading = false;
    });
  }

  editStore = () => {

  }

  saveSections = () => {

  }

  newSection = () => {

  }

  drop = (event: CdkDragDrop<Section[]>) => {
    this.ngxsStore.dispatch(new DropSection(event.previousIndex, event.currentIndex));
  }

  ngOnDestroy(): void {
    if (this.#routeSubscription) {
      this.#routeSubscription.unsubscribe();
    }
  }
}
