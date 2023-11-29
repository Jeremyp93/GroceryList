using GroceryList.Domain.Aggregates.GroceryLists;
using MongoDB.Bson.Serialization;

namespace GroceryList.Infrastructure.DataBase;
public class GroceryListMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Domain.Aggregates.GroceryLists.GroceryList>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapMember(x => x.Ingredients);
            map.MapIdMember(x => x.Id);
        });

        BsonClassMap.RegisterClassMap<Ingredient>(map =>
        {
            map.AutoMap();
        });
    }
}
