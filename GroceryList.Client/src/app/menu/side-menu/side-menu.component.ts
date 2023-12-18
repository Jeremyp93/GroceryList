import { Component, Renderer2, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationStart, Router, RouterModule } from '@angular/router';

import { AuthState } from '../../auth/ngxs-store/auth.state';
import { SideMenuService } from './side-menu.service';

@Component({
  selector: 'app-side-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.scss',
  animations: [
    trigger('sidebarAnimation', [
      transition(':enter', [
        style({
          transform: 'translateX(-100%)',
          opacity: 0
        }),
        animate('0.3s ease-in-out', style({
          transform: 'translateX(0)',
          opacity: 1
        }))
      ]),
      transition(':leave', [
        animate('0.3s ease-in-out', style({
          transform: 'translateX(-100%)',
          opacity: 0
        }))
      ])
    ])
  ]
})
export class SideMenuComponent implements OnInit {
  #router = inject(Router);
  #sideMenuService = inject(SideMenuService);
  #renderer = inject(Renderer2);
  isOpen = false;
  @Select(AuthState.getName) name$!: Observable<string>;

  ngOnInit(): void {
    this.#router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.#sideMenuService.closeMenu();
      }
    });
    this.#sideMenuService.isOpen$.subscribe(open => {
      if (open) {
        this.#renderer.addClass(document.body, 'sidebar-open');
      } else {
        this.#renderer.removeClass(document.body, 'sidebar-open');
      }
      this.isOpen = open;
    });
  }
}
