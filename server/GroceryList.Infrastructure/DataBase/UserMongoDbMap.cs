using GroceryList.Domain.Aggregates.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GroceryList.Infrastructure.DataBase;
public class UserMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<User>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });
    }
}
