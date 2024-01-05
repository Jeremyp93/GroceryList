import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroceryListNewComponent } from './grocery-list-new.component';

describe('GroceryListNew2Component', () => {
  let component: GroceryListNewComponent;
  let fixture: ComponentFixture<GroceryListNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GroceryListNewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroceryListNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
