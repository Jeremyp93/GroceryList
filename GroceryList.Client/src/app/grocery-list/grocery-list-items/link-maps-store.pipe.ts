import { Pipe, PipeTransform } from '@angular/core';
import { Store } from '../../store/types/store.type';
import { GOOGLE_MAPS_QUERY, GEO_MOBILE_QUERY } from '../../constants';

@Pipe({
    name: 'linkMapsStore',
    standalone: true
})
export class LinkMapsStorePipe implements PipeTransform {
    transform(store: Store): string | undefined {
        if (!store) return;

        if (/Mobi|Android/i.test(navigator.userAgent)) {
            return `${GEO_MOBILE_QUERY}${store.street}, ${store.city} ${store.zipCode}, ${store.country}`;
        } else {
            return `${GOOGLE_MAPS_QUERY}${store.street}, ${store.city} ${store.zipCode}, ${store.country}`;
        }
    }
}