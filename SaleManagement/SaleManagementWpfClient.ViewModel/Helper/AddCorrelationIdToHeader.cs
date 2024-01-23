using Serilog;
using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagementWpfClient.Helper
{

    public class AddCorrelationIdToHeader : DelegatingHandler
    {
        ContextCorrelator contextCorrelator;
        public AddCorrelationIdToHeader(ContextCorrelator contextCorrelator)
        {
            this.contextCorrelator = contextCorrelator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string correlationId = null;
            if (contextCorrelator == null)
            {
                Log.Error("contextCorrelator is null - not able to marshall correlation ID to WebApi - pls fix DI");
            }
            else
            {
                correlationId = contextCorrelator.GetValue(HttpHeaderNames.CORRELATIONID);
                if (string.IsNullOrEmpty(correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();

                }
                Log.Debug("Calling endpoint {RequestUri} with correlation id {CORRELATIONID}", request.RequestUri, correlationId);
                request.Headers.Add(HttpHeaderNames.CORRELATIONID, correlationId);
                
            }
            var response = await base.SendAsync(request, cancellationToken);

            Log.Debug("Calling endpoint {RequestUri} with correlation id {CORRELATIONID} completed", request.RequestUri, correlationId);
            return response;
        }
    }
}
