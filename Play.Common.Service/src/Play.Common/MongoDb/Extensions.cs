using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Interfaces;
using Play.Common.Settings;

namespace Play.Common.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            //
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            //
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            //
            services.AddSingleton(ServiceProvider =>
            {
                var Configuration = ServiceProvider.GetService<IConfiguration>();
                var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });
            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string CollectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(servicesProvider =>
            {
                var database = servicesProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, CollectionName);
            });
            return services;
        }
    }
}