using GroceryList.Domain.Aggregates.Users;
using MongoDB.Bson.Serialization;

namespace GroceryList.Infrastructure.DataBase;
public class UserMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<User>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
        });
    }
}
