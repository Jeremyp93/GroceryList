// See https://aka.ms/new-console-template for more information
using GroceryList.Console.Helpers;
using GroceryList.WebApi.Models.GroceryLists;
using GroceryList.WebApi.Models.Stores;

await SetupData();

async Task SetupData()
{
    Console.WriteLine("----- Process started -----");
    var storeRequest = new StoreRequest
    {
        Name = "Maxi",
        Sections = new List<GroceryList.Application.Commands.Stores.SectionDto>
        {
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Vegetables", Priority = 1 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Fruits", Priority = 1 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Bread", Priority = 2 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Charcuterie", Priority = 3 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Bio", Priority = 4 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Meat", Priority = 5 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Fish", Priority = 6 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Dairy", Priority = 7 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Frozen", Priority = 8 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Candy", Priority = 9 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Dry", Priority = 10 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Pet", Priority = 11 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Deco", Priority = 12 },

        },
        Street = "375 Rue Jean-Talon O",
        City = "Montreal",
        ZipCode = "H3N 2Y8",
        State = "QC",
        Country = "Canada"
    };
    Console.WriteLine("----- test -----");
    var storeMaxi = await RequestHelper.Add<StoreResponse>("stores", storeRequest);
    Console.WriteLine("----- First store created -----");
    storeRequest = new StoreRequest
    {
        Name = "Marché Jean-Talon",
        Sections = new List<GroceryList.Application.Commands.Stores.SectionDto>
        {
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Vegetables", Priority = 1 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Fruits", Priority = 1 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Bread", Priority = 2 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Cheese", Priority = 3 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Meat", Priority = 4 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Nuts", Priority = 5 },
            new GroceryList.Application.Commands.Stores.SectionDto { Name = "Books", Priority = 6 }

        },
        Street = "7070 Av. Henri-Julien",
        City = "Montreal",
        ZipCode = "H2S 3S3",
        State = "QC",
        Country = "Canada"
    };
    var storeJeanTalon = await RequestHelper.Add<StoreResponse>("stores", storeRequest);
    Console.WriteLine("----- Second store created -----");

    var listRequest = new GroceryListRequest
    {
        Name = "List Maxi - course semaine",
        UserId = Guid.NewGuid(),
        StoreId = storeMaxi.Id,
        Ingredients = new List<GroceryList.Application.Commands.GroceryLists.IngredientDto>
        {
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Gravier litiere", Amount = 1, Category = "Pet" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Poivron", Amount = 4, Category = "Vegetables" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Baguette", Amount = 1, Category = "Bread" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Lait", Amount = 1, Category = "Dairy" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Blanc de poulet", Amount = 2, Category = "Meat" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Crevettes", Amount = 1, Category = "Fish" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Tomate", Amount = 5, Category = "Vegetables" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Banane", Amount = 2, Category = "Fruits" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Spaghetti", Amount = 2, Category = "Dry" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Pizza 4 fromage", Amount = 1, Category = "Frozen" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Courgette", Amount = 3, Category = "Vegetables" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Chips bbq", Amount = 1, Category = "Candy" },
        }
    };

    await RequestHelper.Add<GroceryListResponse>("grocerylists", listRequest);
    Console.WriteLine("----- First list created -----");

    listRequest = new GroceryListRequest
    {
        Name = "List Jean-Talon - extra",
        UserId = Guid.NewGuid(),
        StoreId = storeJeanTalon.Id,
        Ingredients = new List<GroceryList.Application.Commands.GroceryLists.IngredientDto>
        {
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Noix de cajou", Amount = 1, Category = "Nuts" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Fromage raclette", Amount = 1, Category = "Cheese" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Pain de campagne", Amount = 1, Category = "Bread" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Pommes de terre", Amount = 5, Category = "Vegetables" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Raisin blanc", Amount = 1, Category = "Fruits" },
        }
    };

    await RequestHelper.Add<GroceryListResponse>("grocerylists", listRequest);
    Console.WriteLine("----- Second list created -----");

    listRequest = new GroceryListRequest
    {
        Name = "List Jean-Talon - extra de extra",
        UserId = Guid.NewGuid(),
        StoreId = storeJeanTalon.Id,
        Ingredients = new List<GroceryList.Application.Commands.GroceryLists.IngredientDto>
        {
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Noix de cajou", Amount = 1, Category = "Nuts" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Fromage raclette", Amount = 1, Category = "Cheese" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Pain de campagne", Amount = 1, Category = "Bread" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Pommes de terre", Amount = 5, Category = "Vegetables" },
            new GroceryList.Application.Commands.GroceryLists.IngredientDto { Name = "Raisin blanc", Amount = 1, Category = "Fruits" },
        }
    };

    await RequestHelper.Add<GroceryListResponse>("grocerylists", listRequest);
    Console.WriteLine("----- Process ended -----");
}
