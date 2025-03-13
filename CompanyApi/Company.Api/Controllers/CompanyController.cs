using Company.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CompanyResponse>>> GetCars()
        {
            //var cars = await retrieveCarApplicationService.RetrieveAllCars();
            var cars = new List<CompanyResponse>
            {
                new CompanyResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 1",
                    Exchange = "Exchange 1",
                    Ticker = "Ticker 1",
                    Isin = "Isin 1",
                    Website = "Website 1"
                },
                new CompanyResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 2",
                    Exchange = "Exchange 2",
                    Ticker = "Ticker 2",
                    Isin = "Isin 2",
                    Website = "Website 2"
                }
            };

            return Ok(cars);
        }
    }
}
