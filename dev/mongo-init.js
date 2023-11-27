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
    address: {
      street: "375 Rue Jean-Talon O",
      city: "Montreal",
      zipCode: "H3N 2Y8",
      state: "QC",
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
    address: {
      street: "7070 Av. Henri-Julien",
      city: "Montreal",
      zipCode: "H2S 3S3",
      state: "QC",
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
        _id: "456151ee-f14c-43a3-b332-a789a44f3cc9",
        name: "Gravier litiere",
        amount: 1,
        category: { name: "Pet" },
      },
      {
        _id: "7556c86b-951d-4c2f-9ed7-297a6aac6da1",
        name: "Poivron",
        amount: 4,
        category: { name: "Vegetables" },
      },
      {
        _id: "6fb6f827-6838-4478-97ec-37810cabb9bf",
        name: "Baguette",
        amount: 1,
        category: { name: "Bread" },
      },
      {
        _id: "4381d97b-5077-44fa-a868-6e77c61221b5",
        name: "Lait",
        amount: 3,
        category: { name: "Dairy" },
      },
      {
        _id: "785d82a1-6c63-4145-a64b-02690d93e12c",
        name: "Blanc de poulet",
        amount: 2,
        category: { name: "Meat" },
      },
      {
        _id: "2d780728-76b9-4ab7-8a0c-bf54c93499ef",
        name: "Crevettes",
        amount: 1,
        category: { name: "Fish" },
      },
      {
        _id: "cb3f6276-fd6e-4e3c-8799-54fda34298f2",
        name: "Tomate",
        amount: 2,
        category: { name: "Vegetables" },
      },
      {
        _id: "3bace5e0-8063-4828-919a-f9e6a32e2b1e",
        name: "Banane",
        amount: 3,
        category: { name: "Fruits" },
      },
      {
        _id: "be49a620-32c1-4bb0-a12c-a1e5aaffaabc",
        name: "Spaghetti",
        amount: 1,
        category: { name: "Dry" },
      },
      {
        _id: "7f356df0-368d-463a-9f62-fa27e9659966",
        name: "Pizza 4 fromage",
        amount: 2,
        category: { name: "Frozen" },
      },
      {
        _id: "9709f23a-3e61-4bf8-887c-3b2b485a5126",
        name: "Courgette",
        amount: 1,
        category: { name: "Vegetables" },
      },
      {
        _id: "28ea147c-37dd-471f-b2c7-6dbca5a8bbc7",
        name: "Chips bbq",
        amount: 2,
        category: { name: "Candy" },
      },
    ],
    createdAt: "2023-11-23T15:32:45+00:00",
  },
  {
    _id: BinData(3, "ViuW04NJcEKrBKuznIulIQ=="),
    name: "List Jean-Talon",
    userId: BinData(3, "N5m5cCElO0WZ70nw105Ssg=="),
    storeId: BinData(3, "vh8kTF6uOEibqmhIn7kCMg=="),
    ingredients: [
      {
        _id: "8e5734e4-e60d-48bf-881e-6bae7fca195c",
        name: "Noix de cajou",
        amount: 1,
        category: { name: "Nuts" },
      },
      {
        _id: "5a669e14-33d0-484b-8e07-d47ed078f6c1",
        name: "Fromage raclette",
        amount: 1,
        category: { name: "Cheese" },
      },
      {
        _id: "5a893650-4738-4606-813e-dbaf4aaccb6e",
        name: "Pain de campagne",
        amount: 1,
        category: { name: "Bread" },
      },
      {
        _id: "9b6d7cdf-520d-4888-8dac-89b817fbca33",
        name: "Pommes de terre",
        amount: 3,
        category: { name: "Vegetables" },
      },
      {
        _id: "3c474ea9-fee8-4ebb-82cd-4176d2cf7f8c",
        name: "Raisin blanc",
        amount: 2,
        category: { name: "Fruits" },
      },
    ],
    createdAt: "2023-11-25T15:32:45+00:00",
  },
]);
