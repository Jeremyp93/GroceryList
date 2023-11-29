using GroceryList.Domain.Aggregates.Stores;
using MongoDB.Bson.Serialization;

namespace GroceryList.Infrastructure.DataBase;

public class StoreMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Store>(map =>
        {
            map.AutoMap();
            //map.MapIdMember(x => x.Id);
            map.SetIgnoreExtraElements(true);
        });
    }
}
