using GroceryList.Domain.Aggregates.Products;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GroceryList.Infrastructure.DataBase;
public class ProductMongoDbMap
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Product>(map =>
        {
            map.AutoMap();
            map.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            map.MapMember(x => x.Category);
            map.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<ProductCategory>(map =>
        {
            map.AutoMap();
        });
    }
}