using WebApi.Client;

namespace WebApi.Test
{
    public class WebApiTest
    {
        string _connectstring = "Data Source=localhost;Initial Catalog=SaleManagement2;Integrated Security=True;Encrypt=optional;";


        [Fact]
        public async void GetAllDistrictsTest()
        {
            DistrictClient districtClient = new DistrictClient("http://localhost:5000", new HttpClient());
            var allDistricts = await districtClient.GetAllDistrictsAsync();

            Assert.Equal(4, allDistricts.Count);
            var district1 = allDistricts.Where(x => x.Id == 1).Single();
            Assert.Equal("Distict1", district1.Name);
            Assert.Equal(1, district1.Id);
            
            // Etc
        }
    }
}