using Elasticsearch.API.Models.EcommerceModel;
using Elasticsearch.API.Repositories;
using System.Collections.Immutable;

namespace Elasticsearch.API.Services
{
    public class ECommerceService
    {
        private readonly ECommerceRepository _repository;

        public ECommerceService(ECommerceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IImmutableList<ECommerce>> TermQuery(string customerFirstName)
        {
            return await _repository.TermQuery(customerFirstName);
        }
    }
}
