import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideStore } from '@ngxs/store';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { StoreDetailComponent } from './store-detail.component';
import { StoreState } from '../ngxs-store/store.state';

describe('StoreDetailComponent', () => {
  let component: StoreDetailComponent;
  let fixture: ComponentFixture<StoreDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoreDetailComponent],
      providers: [ provideRouter([]), provideStore([StoreState]), provideHttpClient(), provideHttpClientTesting() ]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StoreDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
