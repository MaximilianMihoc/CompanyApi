using Company.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Api.Data
{
    public class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
    {
        public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();
        public DbSet<ExchangeEntity> Exchanges => Set<ExchangeEntity>();
    }
}
