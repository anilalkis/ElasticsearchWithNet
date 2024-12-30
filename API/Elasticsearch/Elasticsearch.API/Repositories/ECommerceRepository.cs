using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.API.Models.EcommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        private const string indexName = "kibana_sample_data_ecommerce";

        public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
        {
            //tip güvenliği yok
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));

            //tip güvenliği
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //    .Query(q => q.Term(t => t.Field(f => f.CustomerFirstName).Suffix("keyword")),c));

            var termQuery = new TermQuery("customer_first_name.keyword"!)
            {
                CaseInsensitive = true,
                Value = customerFirstName
            };

            var result = await _client.SearchAsync<ECommerce>(s => s.Query(q => q.Term(termQuery)));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerNameList.ForEach(x => terms.Add(x));

            var termsQuery = new TermsQuery()
            {
                Field = "customer_first_name.keyword"!,
                Terms = new TermsQueryField(terms.AsReadOnly())
            };

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q
                .Prefix(p => p
                    .Field(f => f.CustomerFullName
                        .Suffix("keyword")).Value(customerFullName))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q
                .Range(r => r
                    .NumberRange(nr => nr
                        .Field(f => f.TaxFulTotalPrice).Gte(fromPrice).Lte(toPrice)))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q.MatchAll(new MatchAllQuery())));

            return result.Documents.ToImmutableList();
        }
    }
}
