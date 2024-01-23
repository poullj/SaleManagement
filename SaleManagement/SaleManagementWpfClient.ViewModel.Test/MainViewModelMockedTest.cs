using SaleManagementWpfClient.ViewModels;
using Moq;
using WebApi.Client;

namespace SaleManagementWpfClient.ViewModel.Test
{
    public class MainViewModelMockedTest
    {
        private MainViewModel _viewModel;
        private Mock<IDistrictClient> _districtClientMock;
        private Mock<ISalesPersonClient> _salesPersonClientMock;

        public MainViewModelMockedTest()
        {
            _districtClientMock = new Mock<IDistrictClient>();
            var districts = new List<DistrictDTO>() { new DistrictDTO()
            {
                Id = 0,
                Name = "District1",
                SalesPersons = new List<SalesPersonInDistrictDTO>() { new SalesPersonInDistrictDTO() { Id=1, Name="Joe", Primary=true, Secondary=false } },
                Stores = new List<StoreDTO>() { new StoreDTO() { Id=0, Name="Store1"} }

            } };

            _districtClientMock.Setup(x => x.GetAllDistrictsAsync(new CancellationToken())).ReturnsAsync(districts);

            _salesPersonClientMock = new Mock<ISalesPersonClient>();
            var salesPersons = new List<SalesPersonDTO>() { new SalesPersonDTO() { Id = 1, Name = "Lucy" } };
            _salesPersonClientMock.Setup(x => x.GetAllSalesPersonsAsync(new CancellationToken())).ReturnsAsync(salesPersons); 
            _viewModel = new MainViewModel(_districtClientMock.Object, _salesPersonClientMock.Object);
        }

        [Fact]
        public async Task AddSalespersonTest()
        {
            // Arrange: Initialize view model
            await _viewModel.OnLoaded();

            // Act:
            // Select SalesPerson and District
            _viewModel.SelectedSalesPerson = _viewModel.SalesPersons.FirstOrDefault();
            // Add Salesperson to District
            await _viewModel.AddSalesPersonCommand.ExecuteAsync(null);


            // Assert:
            // Verifify that the SalesPerson is in SelectDistrict.Salesperson 


        }
    }
}