using Company.Api.Data.Entities;
using Company.Api.Models;
using System.Linq.Expressions;

namespace Company.Api.Projections
{
    public interface ICompanyProjection
    {
        Expression<Func<CompanyEntity, CompanyResponse>> Project();
    }

    public class CompanyProjection : ICompanyProjection
    {
        public Expression<Func<CompanyEntity, CompanyResponse>> Project()
        {
            return company => new CompanyResponse
            {
                Id = company.Id,
                Name = company.Name,
                ExchangeId = company.ExchangeId,
                Exchange = company.Exchange.Name,
                Ticker = company.Ticker,
                Isin = company.Isin,
                Website = company.Website
            };
        }
    }
}
