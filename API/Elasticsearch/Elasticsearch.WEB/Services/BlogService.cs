using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.ViewModels.BlogViewModels;

namespace Elasticsearch.WEB.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {

            var blog = new Blog();
            blog.Title = model.Title;
            blog.UserId = Guid.NewGuid();
            blog.Tags = model.Tags.Split(",");
            blog.Content = model.Content;

            var isCreated = await _blogRepository.SaveAsync(blog);

            return isCreated != null;
        }

        public Task<List<Blog>> SearchAsync(string searchText)
        {
            return _blogRepository.SearchAsync(searchText);
        }
    }
}
