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
    _id: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
    firstName: "Jeremy",
    lastName: "Proot",
    email: "jeremy.proot@outlook.com",
    password:
      "AQAAAAAAAAAAAAAAAB0aqgNTKIcpVOstZNbj546bxpa9VOMLZwHFJa3MjQ3Rt2KefDX6A1ocv4ALuGOBCA==",
  },
]);

db.Store.insert([
  {
    _id: BinData(3, "5YhRP31jMk2zT4mg//NOFQ=="),
    name: "Maxi",
    userId: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
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
    _id: BinData(3, "vh8kTF6uOEibqmhIn7kCMg=="),
    name: "Marché Jean-Talon",
    userId: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
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
    _id: BinData(3, "KF1Uwz3/qUS1gOnbU4fwHw=="),
    name: "List Maxi - course semaine",
    userId: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
    storeId: BinData(3, "5YhRP31jMk2zT4mg//NOFQ=="),
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
    _id: BinData(3, "ViuW04NJcEKrBKuznIulIQ=="),
    name: "List Jean-Talon",
    userId: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
    storeId: BinData(3, "vh8kTF6uOEibqmhIn7kCMg=="),
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
