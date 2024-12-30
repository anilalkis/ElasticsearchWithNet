using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.Dtos.ProductDtos;
using Elasticsearch.API.Models;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string IndexName = "products";
        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index(IndexName).Id(Guid.NewGuid().ToString()));

            if (!response.IsSuccess()) return null;

            newProduct.Id = response.Id;

            return newProduct;
        }

        public async Task<IImmutableList<Product>> GetAllAsync()
        {
            //var result = await _client.SearchAsync<Product>(s => s.Index(IndexName).Query(q => q.MatchAll());

            var result = await _client.SearchAsync<Product>(IndexName);

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(IndexName));

            if (!response.IsSuccess()) return null;

            response.Source.Id = response.Id;

            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(productUpdateDto.Id, x => x.Index(IndexName).Doc(productUpdateDto));

            return response.IsSuccess();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id,x=>x.Index(IndexName));

            return response.IsSuccess();
        }
    }
}
