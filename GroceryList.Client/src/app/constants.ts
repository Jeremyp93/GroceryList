export const GOOGLE_MAPS_QUERY: string = 'https://www.google.com/maps?q=';
export const GOOGLE_MAPS_QUERY_ANDROID: string = 'https://maps.google.com/?q=';
export const MAPS_QUERY_IPHONE: string = 'http://maps.apple.com/?q=';
export const GEO_MOBILE_QUERY: string = 'geo:0,0?q=';

export const ROUTES_PARAM = {
    GROCERY_LIST: {
        GROCERY_LIST: 'grocery-lists',
        EDIT: 'edit',
        EDIT2: 'edit2',
        NEW: 'new',
        NEW2: 'new2'
    },
    STORE: {
        STORE: 'stores',
        NEW: 'new',
        EDIT: 'edit'
    },
    ITEMS: {
        ITEMS: 'items'
    },
    AUTHENTICATION: {
        LOGIN: 'login',
        REGISTER: 'register',
        LOGOUT: 'logout',
        ME: 'me'
    },
    OAUTH: {
        OAUTH: 'oauth',
        TWITCH_CALLBACK: 'twitch/callback'
    },
    CONFIRM_EMAIL: 'register/confirm-email',
    ID_PARAMETER: 'id',
}

export const INGREDIENT_FORM = {
    ID: 'id',
    NAME: 'name',
    AMOUNT: 'amount',
    CATEGORY: 'category'
};

export const GROCERY_LIST_FORM = {
    NAME: 'name',
    STORE_ID: 'storeId',
    INGREDIENTS: 'ingredients'
};

export const LOGIN_FORM = {
    USERNAME: 'username',
    PASSWORD: 'password'
};

export const SIGNUP_FORM = {
    FIRST_NAME: 'firstName',
    LAST_NAME: 'lastName',
    EMAIL: 'email',
    PASSWORD: 'password',
    CONFIRM_PASSWORD: 'confirmPassword'
};

export const STORE_FORM = {
    NAME: 'name',
    STREET: 'street',
    ZIPCODE: 'zipCode',
    CITY: 'city',
    COUNTRY: 'country',
    SECTIONS: 'sections'
};

export const SECTION_FORM = {
    ID: 'id',
    PRIORITY: 'priority',
    NAME: 'name'
};

export const RESET_PASSWORD_FORM = {
    PASSWORD: 'password',
    CONFIRM_PASSWORD: 'confirmPassword'
};