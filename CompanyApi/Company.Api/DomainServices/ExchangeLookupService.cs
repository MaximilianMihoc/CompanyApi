using Company.Api.Data;
using Company.Api.Data.Entities;
using Company.Api.Models;
using Company.Api.Utils;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Company.Api.DomainServices
{
    public interface IExchangeLookupService
    {
        Task<List<ExchangeResponse>> GetAllExchanges();
        Task<ExchangeResponse?> GetExchangeById(Guid id);
        Task<ResponseBuilder<CreatedResponse>> CreateExchange(ExchangeSubmission request);
    }

    public class ExchangeLookupService : IExchangeLookupService
    {
        private readonly CompanyDbContext dbContext;
        private readonly ILogger<ExchangeLookupService> logger;

        public ExchangeLookupService(CompanyDbContext dbContext, ILogger<ExchangeLookupService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<ExchangeResponse>> GetAllExchanges()
        {
            return await dbContext.Exchanges
                .Select(e => new ExchangeResponse { Id = e.Id, Name = e.Name })
                .ToListAsync();
        }

        public async Task<ExchangeResponse?> GetExchangeById(Guid id)
        {
            return await dbContext.Exchanges
                .Where(e => e.Id == id)
                .Select(e => new ExchangeResponse { Id = e.Id, Name = e.Name })
                .FirstOrDefaultAsync();
        }

        public async Task<ResponseBuilder<CreatedResponse>> CreateExchange(ExchangeSubmission request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ResponseBuilder<CreatedResponse>()
                    .WithError("Exchange name is required.", HttpStatusCode.BadRequest);
            }

            var existingExchange = await dbContext.Exchanges.AnyAsync(e => e.Name == request.Name);
            if (existingExchange)
            {
                return new ResponseBuilder<CreatedResponse>()
                    .WithError($"Exchange '{request.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var newExchange = new ExchangeEntity { Name = request.Name };
            dbContext.Exchanges.Add(newExchange);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Created new exchange: {ExchangeName}", newExchange.Name);

            return new ResponseBuilder<CreatedResponse>()
                .WithSuccess(new CreatedResponse { Id = newExchange.Id }, HttpStatusCode.Created);
        }
    }
}
