using Elasticsearch.API.Dtos;
using Elasticsearch.API.Dtos.ProductDtos;
using Elasticsearch.API.Repositories;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _repository.SaveAsync(request.CreateProduct());

            if(response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında bir hata meydana geldi." },HttpStatusCode.InternalServerError); 
            }

            return ResponseDto<ProductDto>.Succsess(response.CreateDto(), HttpStatusCode.Created);
        }
    }
}
