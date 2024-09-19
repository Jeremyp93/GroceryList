import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../product.service';
import { Category } from '../types/category.type';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent implements OnInit {
  #productService = inject(ProductService);
  categories: Category[] = [];

  async ngOnInit(): Promise<void> {
    this.categories = await lastValueFrom(this.#productService.getAllCategories());
  }
}
