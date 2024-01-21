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
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;
using static Dapper.SqlMapper;

namespace DataAccessLayer
{
    public class Repository
    {
        private string _connectstring;

        public Repository(string connectstring)
        {
            _connectstring = connectstring;
        }


        public async Task<List<SalesPersonDTO>> GetAllSalesPersons()
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                dbConnection.Open();
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    return (await dbConnection.QueryAsync<SalesPersonDTO>(sql: "dbo.spGetAllSalesPersons", commandType: CommandType.StoredProcedure)).ToList();
                }
            }
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
                            var salesPerson = salesPersonTemp[district.PrimarySalesPersonId];
                            var newDistrict = new DistrictDTO() 
                            {   
                                Id = district.Id, 
                                Name = district.Name, 
                                SalesPersons = {new SalesPersonInDistrictDTO()
                                {
                                    Id = salesPerson.Id,
                                    Name = salesPerson.Name,
                                    Primary = true,
                                    Secondary = false
                                }}
                            };
                            districtDTOs.Add(district.Id, newDistrict);

                        }

                        foreach (var districtSalesPerson in districtSalesPersonTemp)
                        {
                            var salesPerson = salesPersonTemp[districtSalesPerson.SalesPersonId];
                            var district = districtTemp[districtSalesPerson.DistrictId];

                            var salesPersonDto = districtDTOs[districtSalesPerson.DistrictId].SalesPersons.Where(x => x.Id == salesPerson.Id).SingleOrDefault();
                            if (salesPersonDto == null)
                            {
                                salesPersonDto = new SalesPersonInDistrictDTO()
                                {
                                    Id = salesPerson.Id,
                                    Name = salesPerson.Name,
                                    Primary = false,
                                    Secondary = districtSalesPerson.Secondary
                                };
                                districtDTOs[districtSalesPerson.DistrictId].SalesPersons.Add(salesPersonDto);
                            }
                            else
                            {
                                salesPersonDto.Secondary = districtSalesPerson.Secondary;
                            }
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

        public async Task AddSalesPersonToDistrict(int districtId, int salesPersonID, bool primary=false, bool secondary = false)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    dbConnection.Open();
                    using (SqlCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandType = CommandType.StoredProcedure;

                        dbCommand.CommandText = "dbo.spAddSalesPersonToDistrict";

                        dbCommand.Parameters.AddWithValue("@SalesPersonID", salesPersonID);
                        dbCommand.Parameters.AddWithValue("@DistrictID", districtId);
                        dbCommand.Parameters.AddWithValue("@Secondary", secondary);

                        await dbCommand.ExecuteNonQueryAsync();
                    }
                    if (primary)
                    {
                        using (SqlCommand dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.CommandType = CommandType.StoredProcedure;

                            dbCommand.CommandText = "dbo.spMakeSalesPersonPrimaryInDistrict";

                            dbCommand.Parameters.AddWithValue("@SalesPersonID", salesPersonID);
                            dbCommand.Parameters.AddWithValue("@DistrictID", districtId);

                            await dbCommand.ExecuteNonQueryAsync();
                        }
                    }
                    scope.Complete();
                }
            }
        }



        public async Task RemoveSalesPersonFromDistrict(int districtId, int salesPersonID)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectstring))
            {
                dbConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@DistrictID", districtId);
                var district = (await dbConnection.QueryAsync<District>(sql: "dbo.spGetDistrict", param: dynamicParameters, commandType: CommandType.StoredProcedure)).Single();
                if (district.PrimarySalesPersonId == salesPersonID)
                {
                    throw new Exception("Cannot remove primary sales person from district - role has to be reassigned before");
                }

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
