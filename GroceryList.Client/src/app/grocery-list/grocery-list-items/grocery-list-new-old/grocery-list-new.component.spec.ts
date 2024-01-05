import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroceryListNewOldComponent } from './grocery-list-new.component';
import { NgxsModule } from '@ngxs/store';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

describe('GroceryListNewComponent', () => {
  let component: GroceryListNewOldComponent;
  let fixture: ComponentFixture<GroceryListNewOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GroceryListNewOldComponent, NgxsModule.forRoot(), RouterTestingModule],
      providers: [ HttpClient, HttpHandler ]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroceryListNewOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
