using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RxWebCoreSampleApp.Models;
using RxWebCoreSampleApp.Models.Extensions;
using RxWeb.Core.Data;
using System;
using System.Linq;

namespace RxWebCoreSampleApp.BoundedContext.SqlContext
{
    public abstract class BaseBoundedDbContext : DbContext
    {
        public BaseBoundedDbContext(DbContext dbContext, DatabaseConfig databaseConfig, IHttpContextAccessor contextAccessor)
        {
            DbContext = dbContext;
            DatabaseConfig = databaseConfig;
            ContextAccessor = contextAccessor;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(DbContext.Database.GetDbConnection(),
                options =>
                {
                    options.AddConnectionResiliency(this.DatabaseConfig.ConnectionResiliency);
                    options.CommandTimeout(this.DatabaseConfig.CommandTimeout);
                }).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddPropertyValueConversion();
            var tenantId = GetTenantId();
            if (tenantId != 0)
                modelBuilder.AddTenantFilter<int>(tenantId);
        }

        private int GetTenantId()
        {
            var claim = this.ContextAccessor.HttpContext.User.Claims.Where(t => t.Type == "http://rxweb.io/identity/claims/singletenantvalue").FirstOrDefault();
            return claim != null ? Convert.ToInt32(claim.Value) : 0;
        }
        private DbContext DbContext { get; set; }

        private DatabaseConfig DatabaseConfig { get; set; }

        private IHttpContextAccessor ContextAccessor { get; set; }
    }
}

