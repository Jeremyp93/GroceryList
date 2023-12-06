import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutocompleteService } from './autocomplete.service';
import { Subject, debounceTime, distinctUntilChanged, lastValueFrom, switchMap } from 'rxjs';
import { AutocompleteAddress } from './autocomplete.type';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-address-autocomplete',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './address-autocomplete.component.html',
  styleUrl: './address-autocomplete.component.scss',
})
export class AddressAutocompleteComponent implements OnInit {
  @Output() addressSelected = new EventEmitter<AutocompleteAddress>();
  autocompleteService = inject(AutocompleteService);
  searchText = '';
  filteredAddresses: AutocompleteAddress[] = [];
  #searchTerms = new Subject<string>();

  ngOnInit(): void {
    this.#searchTerms.pipe(
      debounceTime(500), // Adjust delay time in milliseconds (e.g., 300ms)
      distinctUntilChanged(),
      switchMap((term: string) => this.autocompleteService.autocomplete(term))
    ).subscribe((addresses: any[]) => {
      this.filteredAddresses = addresses;
    });
  }

  onInputChange(event: Event): void {
    this.#searchTerms.next((event.target as HTMLInputElement).value);
  }

  /* searchAddresses = async () => {
    this.filteredAddresses = await lastValueFrom(this.autocompleteService.autocomplete(this.searchText));
  } */

  selectAddress(address: AutocompleteAddress) {
    // Handle selection of the address, e.g., fill form fields
    this.searchText = address.formatted;
    this.filteredAddresses = []; // Clear the suggestions
    this.addressSelected.emit(address);
  }
}
