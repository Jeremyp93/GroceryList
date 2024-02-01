using GroceryList.Domain.Aggregates.GroceryLists;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

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
            map.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.StoreId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });

        BsonClassMap.RegisterClassMap<Ingredient>(map =>
        {
            map.AutoMap();
        });
    }
}
