db.createUser({
  user: process.env.MONGO_GROCERYLIST_USERNAME,
  pwd: process.env.MONGO_GROCERYLIST_PASSWORD,
  roles: [
    {
      role: "readWrite",
      db: process.env.MONGO_INITDB_DATABASE,
    },
  ],
});

db = new Mongo().getDB(process.env.MONGO_INITDB_DATABASE);

db.createCollection("User", { capped: false });
db.createCollection("Store", { capped: false });
db.createCollection("GroceryList", { capped: false });
