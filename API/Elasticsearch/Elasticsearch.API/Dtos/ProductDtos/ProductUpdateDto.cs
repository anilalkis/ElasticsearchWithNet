namespace Elasticsearch.API.Dtos.ProductDtos
{
    public record ProductUpdateDto(string Id,string Name, decimal Price, int Stock, ProductFeatureDto productFeature)
    {
    }
}
