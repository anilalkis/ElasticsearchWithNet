using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.API.Extentions
{
    public static class Elasticsearch
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var userPassword = configuration.GetSection("Elastic")["Password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!))
                .Authentication(new BasicAuthentication(userName!, userPassword!));

            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
    }
}
