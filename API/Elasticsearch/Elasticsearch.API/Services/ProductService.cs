using Elasticsearch.API.Dtos;
using Elasticsearch.API.Dtos.ProductDtos;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repositories;
using Nest;
using System.Collections.Immutable;
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

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var values = await _repository.GetAllAsync();
            var productListDto = new List<ProductDto>();

            foreach(var x in values) 
            {
                if(x.Feature is null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
                }
                else
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));

                }
            }

            return ResponseDto<List<ProductDto>>.Succsess(productListDto, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var value = await _repository.GetByIdAsync(id);

            if (value == null) return ResponseDto<ProductDto>.Fail("Ürün bulunamadı",HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Succsess(value.CreateDto(),HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productUpdate)
        {
            var isSuccsess = await _repository.UpdateAsync(productUpdate);

            if (!isSuccsess) return ResponseDto<bool>.Fail("Ürün güncellenemedi", HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Succsess(true, HttpStatusCode.NoContent);
        }
    }
}
