using Dapper;
using DataAccessLayer.DatabaseClasses;
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
            var district1 = allDistricts.Where(x => x.Id == 1).Single();
            Assert.Equal("Distict1", district1.Name);
            Assert.Equal(1, district1.Id);
            var primaryInDistrict1 = district1.SalesPersons.Where(x=>x.Id==1).Single();
            var secondaryInDistrict1 = district1.SalesPersons.Where(x => x.Id == 4).Single();
            Assert.Equal("Joe", primaryInDistrict1.Name);
            Assert.True(primaryInDistrict1.Primary);
            Assert.True(secondaryInDistrict1.Secondary);

            var district3 = allDistricts.Where(x => x.Id == 3).Single();
            var unAssignedInDistrict3 = district3.SalesPersons.Where(x => x.Id == 3).Single();
            
            Assert.False(unAssignedInDistrict3.Primary);
            Assert.False(unAssignedInDistrict3.Secondary);
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
        public async void UpdateSalesPersonRoleInDistrictTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            var allDistricts = await repository.GetAllDistricts();
            var district = allDistricts[0];
            var salesPerson = district.SalesPersons.Where(x=>x.Id==4).Single();
            var primaryInDistrict1 = district.SalesPersons.Where(x => x.Id == 1).Single();
            Assert.True(primaryInDistrict1.Primary);

            Assert.Equal("Ellen", salesPerson.Name);
            Assert.False(salesPerson.Primary);
            Assert.True(salesPerson.Secondary);

            await repository.AddSalesPersonToDistrict(districtId: district.Id, 
                                                      salesPersonID: salesPerson.Id, primary: true, secondary: false);

            allDistricts = await repository.GetAllDistricts();
            district = allDistricts[0];
            var formerPrimaryInDistrict1 = district.SalesPersons.Where(x => x.Id == 1).Single();
            Assert.False(formerPrimaryInDistrict1.Primary);

            salesPerson = district.SalesPersons.Where(x => x.Id == 4).Single();
            Assert.Equal("Ellen", salesPerson.Name);
            Assert.True(salesPerson.Primary);
            Assert.False(salesPerson.Secondary);
        }

        [Fact]
        public async void RemoveSalesPersonFromDistrictTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            await repository.RemoveSalesPersonFromDistrict(3,3);
            var allDistricts = await repository.GetAllDistricts();
            var district3 = allDistricts.Where(x=>x.Id==3).Single();
            Assert.DoesNotContain(allDistricts[3].SalesPersons, x=>x.Id==3);
        }

               

        //// Test exceptions
        [Fact]
        public async void RemoveSalesPerson_ZeroPrimariesTest()
        {
            RestoreTestData();
            Repository repository = new Repository(_connectstring);
            await Assert.ThrowsAsync<Exception>(async () => await repository.RemoveSalesPersonFromDistrict(4, 4));
        }
    }
}