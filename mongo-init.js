db.createUser({
  user: "grocerylist",
  pwd: "Triage-Scorer-Sequester9-Existing",
  roles: [
    {
      role: "readWrite",
      db: "grocery_list",
    },
  ],
});

db = new Mongo().getDB("grocery_list");

db.createCollection("user", { capped: false });
db.createCollection("store", { capped: false });
db.createCollection("grocerylist", { capped: false });
