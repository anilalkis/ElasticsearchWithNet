using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
            List<Action<QueryDescriptor<Blog>>> ListQuery = new();


            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll(new MatchAllQuery());

            Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match(m => m
                                 .Field(f => f.Content)
                                 .Query(searchText));

            Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(p => p
                                .Field(f => f.Title)
                                .Query(searchText));

            if (string.IsNullOrEmpty(searchText))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(titleMatchBoolPrefix);
            }

            var result = await _client.SearchAsync<Blog>(s => s.Index("blog")
                .Query(q => q
                    .Bool(b => b
                        .Should(ListQuery.ToArray()))));

            if (result.IsSuccess()) { foreach (var hit in result.Hits) hit.Source!.Id = hit.Id!; }
            return result.Documents.ToList();
        }
    }
}
