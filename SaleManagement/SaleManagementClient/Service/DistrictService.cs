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
    public class DistrictService : HttpClientFactoryBase, IDistrictClient
    {
        private readonly DistrictClient districtClient;
        public DistrictService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IApplicationContext applicationContext, ContextCorrelator contextCorrelator) : base(configuration, httpClientFactory, applicationContext, contextCorrelator)
        {
            districtClient = new DistrictClient(baseUri, httpClient);
        }

        public async Task<ICollection<DistrictDTO>> GetAllDistrictsAsync(CancellationToken cancellationToken = default)
        {
            return await districtClient.GetAllDistrictsAsync();
        }
    }
}
