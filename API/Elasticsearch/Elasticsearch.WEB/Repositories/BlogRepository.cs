using Elastic.Clients.Elasticsearch;
using Elasticsearch.WEB.Models;
using System.Reflection.Metadata.Ecma335;

namespace Elasticsearch.WEB.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blog";

        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Blog> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;

            var response = await _client.IndexAsync(newBlog, x => x.Index(indexName));

            if (!response.IsValidResponse) return null!;
            
            return newBlog;
        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            var result = await _client.SearchAsync<Blog>(s => s.Index("blog")
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            s =>s.Match(m => m
                                 .Field(f => f.Content)
                                 .Query(searchText)),
                            s => s.MatchBoolPrefix(p => p
                                .Field(f => f.Title)
                                .Query(searchText))))));

            return result.Documents.ToList();
        }
    }
}
