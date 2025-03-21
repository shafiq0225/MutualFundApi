using Microsoft.AspNetCore.Mvc;
using Nav.Application.Interfaces;
using Nav.Domain.Entities;

namespace Nav.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavController : ControllerBase
    {
        private readonly INavService _navService;

        public NavController(INavService navService)
        {
            _navService = navService;
        }

        [HttpGet("between-dates")] // users
        public async Task<IActionResult> GetAll([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var navs = await _navService.GetAllNavByDate(startDate, endDate);
            return Ok(navs);
        }

        [HttpGet("visible-funds")] // users
        public async Task<IActionResult> GetVisibleFunds()
        {

            var funds = await _navService.GetVisibleFunds();
            return Ok(funds);
        }

        [HttpGet("visible-scheme")] // users
        public async Task<IActionResult> GetVisibleSchemes([FromQuery] string fundId)
        {

            var funds = await _navService.GetVisibleSchemes(fundId);
            return Ok(funds);
        }

        [HttpGet("GetIndividualNav")] // users
        public async Task<IActionResult> GetIndividualNav([FromQuery] string fundId, [FromQuery] string schemeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var individualNav = await _navService.GetIndividualNav(fundId, schemeId, startDate, endDate);
            return Ok(individualNav);
        }

        [HttpPost] // Admin
        public async Task<IActionResult> Create(IFormFile fund)
        {
            await _navService.AddNavAsync(fund);
            return Ok();
        }

        [HttpPut("{fundId}/{isVisible}")] //Admin
        public async Task<IActionResult> UpdateVisibleFund(string fundId, bool isVisible)
        {
            await _navService.UpdateIsVisible(fundId, isVisible);
            return Ok();
        }

        [HttpPut("{fundId}/{schemeId}/{isVisible}")] //Admin
        public async Task<IActionResult> UpdateIsVisibleInScheme(string fundId, string schemeId, bool isVisible)
        {
            await _navService.UpdateIsVisibleInScheme(fundId, schemeId, isVisible);
            return Ok();
        }
    }
}
