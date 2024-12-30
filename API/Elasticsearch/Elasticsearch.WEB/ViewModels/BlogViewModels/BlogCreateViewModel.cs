using System.Text.Json.Serialization;

namespace Elasticsearch.WEB.ViewModels.BlogViewModels
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Tags { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
