using Microsoft.Extensions.Configuration;
using SaleManagementWpfClient.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Client;

namespace SaleManagementWpfClient.Service
{
    public class SalesPersonService : HttpClientFactoryBase, ISalesPersonClient
    {

        private readonly ISalesPersonClient salesPersonClient;
        public SalesPersonService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IApplicationContext applicationContext, ContextCorrelator contextCorrelator) : base(configuration, httpClientFactory, applicationContext, contextCorrelator)
        {
            salesPersonClient = new SalesPersonClient(baseUri, httpClient);
        }

        public async Task AddSalesPersonToDistrictAsync(SalesPersonRolesDistrictRequest salesPersonRolesDistrictRequest, CancellationToken cancellationToken = default)
        {
            await salesPersonClient.AddSalesPersonToDistrictAsync(salesPersonRolesDistrictRequest, cancellationToken);
        }

        public async Task<ICollection<SalesPersonDTO>> GetAllSalesPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await salesPersonClient.GetAllSalesPersonsAsync(cancellationToken);
        }

        public async Task RemoveSalesPersonFromDistrictAsync(SalesPersonDistrictRequest salesPersonDistrictRequest, CancellationToken cancellationToken = default)
        {
            await salesPersonClient.RemoveSalesPersonFromDistrictAsync(salesPersonDistrictRequest, cancellationToken);
        }
    }
}
