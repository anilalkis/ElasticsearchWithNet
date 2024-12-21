using Elasticsearch.API.Dtos.ProductDtos;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;
        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto request)
        {
            return CreateActionResult(await _productService.UpdateAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> GetAll(string id)
        {
            var values = await _productService.DeleteAsync(id);
            return CreateActionResult(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _productService.GetAllAsync();
            return CreateActionResult(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var values = await _productService.GetByIdAsync(id);
            return CreateActionResult(values);
        }

    }
}
