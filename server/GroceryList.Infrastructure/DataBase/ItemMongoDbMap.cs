using GroceryList.Domain.Aggregates.Items;
using MongoDB.Bson.Serialization;

namespace GroceryList.Infrastructure.DataBase;
public class ItemMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Item>(map =>
        {
            map.AutoMap();
            map.MapMember(item => item.Name).SetIsRequired(true);
            map.SetIgnoreExtraElements(true);
        });
    }
}
