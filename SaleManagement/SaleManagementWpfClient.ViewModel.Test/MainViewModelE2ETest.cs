using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaleManagementWpfClient.Service;
using SaleManagementWpfClient.ViewModels;
using System.Net;
using WebApi.Client;
using Dapper;

namespace SaleManagementWpfClient.ViewModel.Test
{
    public class MainViewModelE2ETest
    {
        private MainViewModel _viewModel;
        private IDistrictClient _districtClient;
        private ISalesPersonClient _salesPersonClient;

        string _connectstring = "Data Source=localhost;Initial Catalog=SaleManagement;Integrated Security=True;Encrypt=optional;";

        void RestoreTestData()
        {
            using (SqlConnection connection = new SqlConnection(_connectstring))
            {
                var dbName = connection.Query<string>("SELECT DB_NAME() AS [Current Database]").SingleOrDefault();
                connection.Query($"alter database {dbName} set single_user with rollback immediate");
                connection.Query($"USE master;RESTORE DATABASE {dbName} FROM DATABASE_SNAPSHOT = '{dbName}_snapshot'");
                connection.Query($"alter database {dbName} set MULTI_user");
            }
        }


        public MainViewModelE2ETest()
        {
            RestoreTestData();

            string baseUrl = "http://localhost:5000";

            _districtClient = new DistrictClient(baseUrl, new HttpClient());
            _salesPersonClient = new SalesPersonClient(baseUrl, new HttpClient());
            _viewModel = new MainViewModel(_districtClient, _salesPersonClient);
        }

        [Fact]
        public async Task AddSalespersonTest()
        {
            // Arrange: Initialize view model
            await _viewModel.OnLoaded();
            _viewModel.SelectedDistrict = _viewModel.Districts[2];
            var newSalesPerson = _viewModel.SalesPersons[1];

            // Check that salesperson 3 is not already in the selected districts salesperson
            Assert.DoesNotContain(_viewModel.SelectedDistrict.SalesPersons, x => x.Id == newSalesPerson.Id);

            // Act:
            // Add Salesperson 3 to District
            _viewModel.SelectedSalesPerson = newSalesPerson;
            await _viewModel.AddSalesPersonCommand.ExecuteAsync(null);

            // Assert:
            // Verifify that the selecte salesperson has been added to the selected districts sales person
            Assert.Contains(_viewModel.SelectedDistrict.SalesPersons, x => x.Id == newSalesPerson.Id);
        }
    }
}