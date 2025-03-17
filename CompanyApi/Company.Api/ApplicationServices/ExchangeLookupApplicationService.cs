using Company.Api.DomainServices;
using Company.Api.Models;
using Company.Api.Utils;
using System.Net;

namespace Company.Api.ApplicationServices
{
    public interface IExchangeLookupApplicationService
    {
        Task<ResponseBuilder<List<ExchangeResponse>>> GetAllExchanges();
        Task<ResponseBuilder<ExchangeResponse?>> GetExchangeById(Guid id);
        Task<ResponseBuilder<CreatedResponse>> CreateExchange(ExchangeSubmission request);
    }

    public class ExchangeLookupApplicationService : IExchangeLookupApplicationService
    {
        private readonly IExchangeLookupService exchangeLookupService;

        public ExchangeLookupApplicationService(IExchangeLookupService exchangeLookupService)
        {
            this.exchangeLookupService = exchangeLookupService;
        }

        public async Task<ResponseBuilder<CreatedResponse>> CreateExchange(ExchangeSubmission request)
        {
            return await exchangeLookupService.CreateExchange(request);
        }

        public async Task<ResponseBuilder<List<ExchangeResponse>>> GetAllExchanges()
        {
            var exchanges = (await exchangeLookupService.GetAllExchanges()).ToList();
            return new ResponseBuilder<List<ExchangeResponse>>().WithSuccess(exchanges, HttpStatusCode.OK);
        }

        public async Task<ResponseBuilder<ExchangeResponse?>> GetExchangeById(Guid id)
        {
            var exchange = await exchangeLookupService.GetExchangeById(id);
            if (exchange == null)
            {
                return new ResponseBuilder<ExchangeResponse?>()
                    .WithError($"Exchange not found.", HttpStatusCode.NotFound);
            }

            return new ResponseBuilder<ExchangeResponse?>().WithSuccess(exchange, HttpStatusCode.OK);
        }
    }
}
