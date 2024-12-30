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

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q
                .Range(r => r
                    .NumberRange(nr => nr
                        .Field(f => f.TaxFulTotalPrice).Gte(fromPrice).Lte(toPrice)))));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q.MatchAll(new MatchAllQuery())));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQuery(string customerName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q
                .Fuzzy(fz => fz
                    .Field(f => f.CustomerFirstName
                        .Suffix("keyword")).Value(customerName)
                            .Fuzziness(new Fuzziness(1)))));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchQueryFullText(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Category)
                        .Query(categoryName))));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExmp1(string cityName, double taxFulTotalPrice, string categoryName, string manufacture)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field("geoip.city_name")
                                .Value(cityName)))
                        .MustNot(mn => mn
                            .Range(r => r
                                .NumberRange(tr => tr
                                    .Field(f => f.TaxFulTotalPrice).Lte(taxFulTotalPrice))))
                        .Should(s => s
                            .Term(t => t
                                .Field(f => f.Category.Suffix("keyword"))
                                    .Value(categoryName)))
                        .Filter(f => f
                            .Term(t => t
                                .Field("manufacturer.keyword")
                                    .Value(manufacture))))));
                        

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }
            return result.Documents.ToImmutableList();
        }
    }
}
