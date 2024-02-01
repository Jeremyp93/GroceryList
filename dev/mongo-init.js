db.createUser({
  user: "grocerylist",
  pwd: "mongo",
  roles: [
    {
      role: "readWrite",
      db: "grocery_list",
    },
  ],
});

db = new Mongo().getDB("grocery_list");

db.createCollection("User", { capped: false });
db.createCollection("Store", { capped: false });
db.createCollection("GroceryList", { capped: false });

db.User.insert([
  {
    _id: UUID('5296cff2-9970-42dd-8f3e-e2688fb48601'),
    userName: "jeremy.proot@outlook.com",
    normalizedUserName: "JEREMY.PROOT@OUTLOOK.COM",
    email: "jeremy.proot@outlook.com",
    normalizedEmail: "JEREMY.PROOT@OUTLOOK.COM",
    emailConfirmed: true,
    passwordHash: "AQAAAAIAAYagAAAAEGARJwZ7q/G57ihI4O76DbXu/mrirZ9y3bz/dfJ9jpSduanFlWRG1LhKb1MEPSx+WA==",
    securityStamp: "XABV5FE7V2I2WYMKWDQUNRRRMPX7FL6H",
    concurrencyStamp: "e199daaa-746d-425e-9373-3809ffcd20a3",
    phoneNumber: null,
    phoneNumberConfirmed: false,
    twoFactorEnabled: false,
    lockoutEnd: null,
    lockoutEnabled: true,
    accessFailedCount: 0,
    version: 1,
    createdOn: ISODate("2024-02-01T20:29:25.619+00:00"),
    claims: [],
    roles: [],
    logins: [],
    tokens: [],
    firstName: "Jeremy",
    lastName: "Proot",
    oAuthProviders: []
  }
]);

db.Store.insert([
  {
    _id: UUID('657cf0f3-755d-4252-bdec-3b18148bacf6'),
    name: "Maxi",
    userId: UUID('5296cff2-9970-42dd-8f3e-e2688fb48601'),
    address: {
      street: "375 Rue Jean-Talon O",
      city: "Montreal",
      zipCode: "H3N 2Y8",
      country: "Canada",
    },
    sections: [
      { name: "Vegetables", priority: 1 },
      { name: "Fruits", priority: 1 },
      { name: "Bread", priority: 2 },
      { name: "Charcuterie", priority: 3 },
      { name: "Bio", priority: 4 },
      { name: "Meat", priority: 5 },
      { name: "Fish", priority: 6 },
      { name: "Dairy", priority: 7 },
      { name: "Frozen", priority: 8 },
      { name: "Candy", priority: 9 },
      { name: "Dry", priority: 10 },
      { name: "Pet", priority: 11 },
      { name: "Deco", priority: 12 },
    ],
  },
  {
    _id: UUID('b2c3032b-7c85-48a9-9a6d-02dfcd020153'),
    name: "Marché Jean-Talon",
    userId: UUID('5296cff2-9970-42dd-8f3e-e2688fb48601'),
    address: {
      street: "7070 Av. Henri-Julien",
      city: "Montreal",
      zipCode: "H2S 3S3",
      country: "Canada",
    },
    sections: [
      { name: "Vegetables", priority: 1 },
      { name: "Fruits", priority: 1 },
      { name: "Bread", priority: 2 },
      { name: "Cheese", priority: 3 },
      { name: "Meat", priority: 4 },
      { name: "Nuts", priority: 5 },
      { name: "Books", priority: 6 },
    ],
  },
]);

db.GroceryList.insert([
  {
    _id: UUID('06f69719-5d5e-43e5-9890-f284472e705b'),
    name: "List Maxi - course semaine",
    userId: UUID('5296cff2-9970-42dd-8f3e-e2688fb48601'),
    storeId: UUID('657cf0f3-755d-4252-bdec-3b18148bacf6'),
    ingredients: [
      {
        name: "Gravier litiere",
        amount: 1,
        category: { name: "Pet" },
        selected: false
      },
      {
        name: "Poivron",
        amount: 4,
        category: { name: "Vegetables" },
        selected: false
      },
      {
        name: "Baguette",
        amount: 1,
        category: { name: "Bread" },
        selected: false
      },
      {
        name: "Lait",
        amount: 3,
        category: { name: "Dairy" },
        selected: false
      },
      {
        name: "Blanc de poulet",
        amount: 2,
        category: { name: "Meat" },
        selected: false
      },
      {
        name: "Crevettes",
        amount: 1,
        category: { name: "Fish" },
        selected: false
      },
      {
        name: "Tomate",
        amount: 2,
        category: { name: "Vegetables" },
        selected: false
      },
      {
        name: "Banane",
        amount: 3,
        category: { name: "Fruits" },
        selected: false
      },
      {
        name: "Spaghetti",
        amount: 1,
        category: { name: "Dry" },
        selected: false
      },
      {
        name: "Pizza 4 fromage",
        amount: 2,
        category: { name: "Frozen" },
        selected: false
      },
      {
        name: "Courgette",
        amount: 1,
        category: { name: "Vegetables" },
        selected: false
      },
      {
        name: "Chips bbq",
        amount: 2,
        category: { name: "Candy" },
        selected: false
      },
    ],
    createdAt: ISODate("2023-11-26T22:40:26.417Z"),
    createdBy: "jeremy.proot@outlook.com",
    lastModifiedAt: ISODate("2023-11-26T22:40:26.417Z"),
    lastModifiedBy: "jeremy.proot@outlook.com",
  },
  {
    _id: UUID('94fb0d4c-29a2-442d-8706-ecac031ac24b'),
    name: "List Jean-Talon",
    userId: UUID('5296cff2-9970-42dd-8f3e-e2688fb48601'),
    storeId: UUID('b2c3032b-7c85-48a9-9a6d-02dfcd020153'),
    ingredients: [
      {
        name: "Noix de cajou",
        amount: 1,
        category: { name: "Nuts" },
        selected: false
      },
      {
        name: "Fromage raclette",
        amount: 1,
        category: { name: "Cheese" },
        selected: true
      },
      {
        name: "Pain de campagne",
        amount: 1,
        category: { name: "Bread" },
        selected: false
      },
      {
        name: "Pommes de terre",
        amount: 3,
        category: { name: "Vegetables" },
        selected: false
      },
      {
        name: "Raisin blanc",
        amount: 2,
        category: { name: "Fruits" },
        selected: false
      },
    ],
    createdAt: ISODate("2023-11-27T22:40:26.417Z"),
    createdBy: "jeremy.proot@outlook.com",
    lastModifiedAt: ISODate("2023-11-27T22:40:26.417Z"),
    lastModifiedBy: "jeremy.proot@outlook.com",
  },
]);
