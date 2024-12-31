using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var isSuccsed = await _blogService.SaveAsync(model);

            if(!isSuccsed)
            {
                TempData["result"] = "kayıt başarısız";
                return RedirectToAction("Save");
            }

            TempData["result"] = "kayıt başarılı";
            return RedirectToAction("Save");

        }

        public async Task<IActionResult> Search()
        {
            return View(await _blogService.SearchAsync(string.Empty));
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            var blogList = await _blogService.SearchAsync(searchText);

            return View(blogList);
        }
    }
}
