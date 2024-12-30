using Elasticsearch.API.Models.EcommerceModel;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerceService _eCommerceService;

        public ECommerceController(ECommerceService eCommerceService)
        {
            _eCommerceService = eCommerceService;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _eCommerceService.TermQuery(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            return Ok(await _eCommerceService.TermsQuery(customerFirstNameList));
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _eCommerceService.PrefixQuery(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _eCommerceService.RangeQuery(fromPrice,toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _eCommerceService.MatchAllQuery());
        }

        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerName)
        {
            return Ok(await _eCommerceService.FuzzyQuery(customerName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string categoryName)
        {
            return Ok(await _eCommerceService.MatchQueryFullText(categoryName));

        }

        public async Task<IActionResult> CompoundQueryExmp1(string cityName, double taxFulTotalPrice, string categoryName, string manufacture)
        {
            return Ok(await _eCommerceService.CompoundQueryExmp1(cityName,taxFulTotalPrice,categoryName,manufacture));

        }
    }
}
