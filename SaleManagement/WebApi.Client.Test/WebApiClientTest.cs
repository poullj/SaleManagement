using Microsoft.AspNetCore.Hosting;

namespace WebApi.Client.Test
{
    public class WebApiClientTest
    {

        public WebApiClientTest()
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddConfiguration(Config);
                }).UseStartup<Startup>();

            testServer = new TestServer(builder);
        }
        [Fact]
        public async void GetAllDistrictsTest()
        {
        }
        
        [Fact]
        public async void AddSalesPersonToDistrictTest()
        {

        }
        
        [Fact]
        public async void UpdateSalesPersonRoleInDistrictTest()
        {

        }
    }
}