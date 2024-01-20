using Dapper;
using DataAccessLayer.DTOs;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.Test
{
    public class RepositoryTest
    {

        string _connectstring = "Data Source=localhost;Initial Catalog=SaleManagement2;Integrated Security=True;Encrypt=optional;";

        void RestoreTestData()
        {
            using (SqlConnection connection = new SqlConnection(_connectstring))
            {
                var dbName = connection.Query<string>("SELECT DB_NAME() AS [Current Database]").SingleOrDefault();
                connection.Query($"alter database {dbName} set single_user with rollback immediate");
                //connection.Query("USE master");
                connection.Query($"USE master;RESTORE DATABASE {dbName} FROM DATABASE_SNAPSHOT = '{dbName}_snapshot'");
                connection.Query($"alter database {dbName} set MULTI_user");
            }
        }

        [Fact]
        public async void GetAllDistrictsTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            var allDistricts = await repository.GetAllDistricts();

            Assert.Equal(4, allDistricts.Count);
            Assert.Equal("Distict1", allDistricts[0].Name);
            Assert.Equal(1, allDistricts[0].Id);
            Assert.Equal("Joe", allDistricts[0].SalesPersons[0].Name);
            Assert.Equal(ResponsibilityEnum.Primary, allDistricts[0].SalesPersons[0].Responsibility);
            Assert.Equal(ResponsibilityEnum.Secondary, allDistricts[0].SalesPersons[1].Responsibility);
            Assert.Null(allDistricts[2].SalesPersons[0].Responsibility);
        }

        [Fact]
        public async void AddSalesPersonToDistrictTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            await repository.AddSalesPersonToDistrict(1, 2);
            var allDistricts = await repository.GetAllDistricts();
            Assert.Equal("Lucy", allDistricts[0].SalesPersons[2].Name);
        }

        [Fact]
        public async void RemoveSalesPersonFromDistrictTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            await repository.RemoveSalesPersonFromDistrict(4,4);
            var allDistricts = await repository.GetAllDistricts();
            Assert.DoesNotContain(allDistricts[3].SalesPersons, x =>x.Name== "Ellen");

        }

        //// Test exceptions
        //[Fact]
        //public void AddSalesPerson_TwoPrimariesTest()
        //{
        //    // TODO
        //}

        //[Fact]
        //public void RemoveSalesPerson_ZeroPrimariesTest()
        //{
        //    // TODO
        //}
    }
}