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

        public async Task<IImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
        {
            return await _repository.TermsQuery(customerFirstNameList);
        }

        public async Task<ImmutableList<ECommerce>> PrefixQuery(string customerFullName)
        {
            return await _repository.PrefixQuery(customerFullName);
        }

        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            return await _repository.RangeQuery(fromPrice,toPrice);

        }

        public async Task<ImmutableList<ECommerce>> MatchAllQuery()
        {
            return await _repository.MatchAllQuery();

        }
    }
}
