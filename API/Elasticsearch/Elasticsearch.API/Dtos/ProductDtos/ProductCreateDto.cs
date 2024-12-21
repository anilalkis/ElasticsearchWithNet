using Elasticsearch.API.Models;

namespace Elasticsearch.API.Dtos.ProductDtos
{
    public record ProductCreateDto(string Name,decimal Price,int Stock, ProductFeatureDto productFeature)
    {

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature()
                {
                    Width = productFeature.Widht,
                    Height = productFeature.Height,
                    Color = (EColor)int.Parse(productFeature.Color)
                }
            };
        }
    }
}
