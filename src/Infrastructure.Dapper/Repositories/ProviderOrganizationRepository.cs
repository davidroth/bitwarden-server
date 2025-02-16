﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bit.Core.Entities.Provider;
using Bit.Core.Models.Data;
using Bit.Core.Repositories;
using Bit.Core.Settings;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bit.Infrastructure.Dapper.Repositories
{
    public class ProviderOrganizationRepository : Repository<ProviderOrganization, Guid>, IProviderOrganizationRepository
    {
        public ProviderOrganizationRepository(GlobalSettings globalSettings)
            : this(globalSettings.SqlServer.ConnectionString, globalSettings.SqlServer.ReadOnlyConnectionString)
        { }

        public ProviderOrganizationRepository(string connectionString, string readOnlyConnectionString)
            : base(connectionString, readOnlyConnectionString)
        { }

        public async Task<ICollection<ProviderOrganizationOrganizationDetails>> GetManyDetailsByProviderAsync(Guid providerId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var results = await connection.QueryAsync<ProviderOrganizationOrganizationDetails>(
                    "[dbo].[ProviderOrganizationOrganizationDetails_ReadByProviderId]",
                    new { ProviderId = providerId },
                    commandType: CommandType.StoredProcedure);

                return results.ToList();
            }
        }

        public async Task<ProviderOrganization> GetByOrganizationId(Guid organizationId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var results = await connection.QueryAsync<ProviderOrganization>(
                    "[dbo].[ProviderOrganization_ReadByOrganizationId]",
                    new { OrganizationId = organizationId },
                    commandType: CommandType.StoredProcedure);

                return results.SingleOrDefault();
            }
        }
    }
}
