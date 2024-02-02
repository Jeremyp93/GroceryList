using GroceryList.Domain.Aggregates.Stores;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GroceryList.Infrastructure.DataBase;

public class StoreMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Store>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.SetIgnoreExtraElements(true);
        });
    }
}
