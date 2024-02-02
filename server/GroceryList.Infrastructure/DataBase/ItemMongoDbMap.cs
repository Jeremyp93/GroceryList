using GroceryList.Domain.Aggregates.Items;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GroceryList.Infrastructure.DataBase;
public class ItemMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Item>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(item => item.Name).SetIsRequired(true);
            map.SetIgnoreExtraElements(true);
        });
    }
}
