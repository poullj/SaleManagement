using DataAccessLayer.DatabaseClasses;
using DataAccessLayer.DTOs;
using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer
{
    public class Repository
    {
        private string _connectstring;

        public Repository(string connectstring)
        {
            _connectstring = connectstring;
        }

        public async Task<List<DistrictDTO>> GetAllDistricts()
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                dbConnection.Open();
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.CommandText = "dbo.spGetAllDistricts";

                    var districtTemp = new Dictionary<int, District>();
                    var salesPersonTemp = new Dictionary<int, SalesPerson>();
                    var storeTemp = new List<Store>();
                    var districtSalesPersonTemp = new List<DistrictSalesPerson>();

                    using (SqlDataReader reader = await dbCommand.ExecuteReaderAsync())
                    {
                        var districtParser = reader.GetRowParser<District>(typeof(District));

                        while (await reader.ReadAsync())
                        {
                            var district = districtParser(reader);
                            districtTemp.Add(district.Id, district);
                        }

                        await reader.NextResultAsync();
                        var salesPersonParser = reader.GetRowParser<SalesPerson>(typeof(SalesPerson));
                        while (await reader.ReadAsync())
                        {
                            var salesPerson = salesPersonParser(reader);
                            salesPersonTemp.Add(salesPerson.Id, salesPerson);
                        }

                        await reader.NextResultAsync();
                        var storeParser = reader.GetRowParser<Store>(typeof(Store));
                        while (await reader.ReadAsync())
                        {
                            var store = storeParser(reader);
                            storeTemp.Add(store);
                        }

                        await reader.NextResultAsync();
                        var districtSalesPersonParser = reader.GetRowParser<DistrictSalesPerson>(typeof(DistrictSalesPerson));
                        while (await reader.ReadAsync())
                        {
                            var districtSalesPerson = districtSalesPersonParser(reader);
                            districtSalesPersonTemp.Add(districtSalesPerson);
                        }
                        Dictionary<int, DistrictDTO> districtDTOs = new();
                        foreach (var district in districtTemp.Values)
                        {
                            districtDTOs.Add(district.Id, new DistrictDTO() { Id = district.Id, Name = district.Name });
                        }

                        foreach (var districtSalesPerson in districtSalesPersonTemp)
                        {
                            var salesPerson = salesPersonTemp[districtSalesPerson.SalesPersonId];
                            var district = districtTemp[districtSalesPerson.DistrictId];
                            ResponsibilityEnum? responsibility = null;
                            if (district.PrimarySalesPersonId == districtSalesPerson.SalesPersonId)
                            {
                                responsibility = ResponsibilityEnum.Primary;
                            }
                            else if (districtSalesPerson.Secondary)
                            {
                                responsibility = ResponsibilityEnum.Secondary;
                            }

                            districtDTOs[districtSalesPerson.DistrictId].SalesPersons.Add(new SalesPersonDTO() { Id = salesPerson.Id, Name = salesPerson.Name, Responsibility = responsibility });
                        }

                        foreach (var store in storeTemp)
                        {
                            districtDTOs[store.DistrictId].Stores.Add(new StoreDTO() { Id = store.Id, Name = store.Name });
                        }
                        return districtDTOs.Values.ToList();
                    }
                }
            }
        }

        public async Task AddSalesPersonToDistrict(int districtId, int salesPersonID, ResponsibilityEnum? responsibility = null)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                dbConnection.Open();
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.CommandText = "dbo.spAddSalesPersonToDistrict";

                    dbCommand.Parameters.AddWithValue("@SalesPersonID", salesPersonID);
                    dbCommand.Parameters.AddWithValue("@DistrictID", districtId);
                    dbCommand.Parameters.AddWithValue("@Secondary", responsibility.HasValue && responsibility.Value == ResponsibilityEnum.Secondary);

                    await dbCommand.ExecuteNonQueryAsync();
                }
            }
        }



        public async Task RemoveSalesPersonFromDistrict(int districtId, int salesPersonID)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                dbConnection.Open();
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.CommandText = "dbo.spRemoveSalesPersonFromDistrict";

                    dbCommand.Parameters.AddWithValue("@SalesPersonID", salesPersonID);
                    dbCommand.Parameters.AddWithValue("@DistrictID", districtId);
                    
                    await dbCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
