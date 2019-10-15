using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RxWebCoreSampleApp.BoundedContext.Singleton;
using RxWebCoreSampleApp.Models;
using RxWebCoreSampleApp.Models.Const;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace RxWebCoreSampleApp.BoundedContext.SqlContext
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(IOptions<DatabaseConfig> databaseConfig, IHttpContextAccessor contextAccessor, TenantDbConnectionInfo tenantDbConnection)
        {
            ConnectionStringConfig = databaseConfig.Value.ConnectionString;
            TenantDbConnection = tenantDbConnection;
            ContextAccessor = contextAccessor;
        }

        public string GetConnection(string keyName)
        {
            return GetDbConnectionString(keyName);
        }

        private string GetDbConnectionString(string keyName)
        {
            var connectionString = string.Empty;
            if (ConnectionStringConfig.ContainsKey(keyName) && !string.IsNullOrEmpty(ConnectionStringConfig[keyName]))
            {
                connectionString = ConnectionStringConfig[keyName];
            }
            else
            {
                var clientId = GetClaimValue(ApplicationConstants.X_CLIENT);
                if (clientId != 0)
                {
                    var clientConfig = TenantDbConnection.Get(clientId);
                    if (clientConfig != null && clientConfig.ContainsKey(keyName))
                        connectionString= clientConfig[keyName];
                }

            }
            return connectionString;
        }

        private int GetClaimValue(string claim)
        {
            var claimObject = ContextAccessor.HttpContext.User.Claims.Where(t => t.Type == claim).SingleOrDefault();
            return (claimObject != null) ? Convert.ToInt32(claimObject.Value) : 0;
        }


        private Dictionary<string, string> ConnectionStringConfig { get; set; }

        private IHttpContextAccessor ContextAccessor { get; set; }

        private TenantDbConnectionInfo TenantDbConnection { get; set; }

    }
}
