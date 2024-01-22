using Microsoft.Extensions.Configuration;
using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagementWpfClient.Helper
{
    public abstract class HttpClientFactoryBase
    {
        #region Protected Members

        protected readonly string httpClientName;
        protected readonly string baseUri;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly HttpClient httpClient;

        #endregion

        
        #region Constructors

        public HttpClientFactoryBase(IConfiguration configuration, IHttpClientFactory httpClientFactory, IApplicationContext applicationContext, ContextCorrelator contextCorrelator)
        {
            this.httpClientFactory = httpClientFactory;
            httpClientName = configuration["APISettings:Name"];
            baseUri = configuration["APISettings:BaseUrl"];
            httpClient = httpClientFactory.CreateClient(httpClientName);
            httpClient.DefaultRequestHeaders.Add(HttpHeaderNames.SessionID, applicationContext.SessionID);
            httpClient.DefaultRequestHeaders.Add(HttpHeaderNames.UserName, applicationContext.UserName);
            httpClient.DefaultRequestHeaders.Add(HttpHeaderNames.Workstation, applicationContext.WorkStation);
            httpClient.DefaultRequestHeaders.Add(HttpHeaderNames.ProcessID, Environment.ProcessId.ToString());
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }

        #endregion


    }
}
