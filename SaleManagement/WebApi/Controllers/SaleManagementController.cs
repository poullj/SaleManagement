using DataAccessLayer;
using DataAccessLayer.DTOs;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Request;

namespace WebApi.Controllers
{
    

    [ApiController]
    [Route("[controller]")]
    public class SaleManagementController : ControllerBase
    {
        string connectstring { get; set; } = "Data Source=localhost;Initial Catalog=SaleManagement;Integrated Security=True;Encrypt=optional;";

        private readonly ILogger<SaleManagementController> _logger;
        
        public SaleManagementController(ILogger<SaleManagementController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllDistricts")]
        public async Task<IEnumerable<DistrictDTO>> Get()
        {
            Repository repository = new Repository(connectstring);
            return await repository.GetAllDistricts();
        }

        [HttpPut("AddSalesPersonToDistrict")]
        public async Task AddSalesPersonToDistrict([FromBody] SalesPersonRolesDistrictRequest salesPersonRolesDistrictRequest)
        {
            Repository repository = new Repository(connectstring);
            await repository.AddSalesPersonToDistrict(salesPersonRolesDistrictRequest.DistrictId,
                                                      salesPersonRolesDistrictRequest.SalesPersonId,
                                                      salesPersonRolesDistrictRequest.Primary,
                                                      salesPersonRolesDistrictRequest.Secondary);
        }

        [HttpPut("RemoveSalesPersonFromDistrict")]
        public async Task RemoveSalesPersonFromDistrict([FromBody] SalesPersonDistrictRequest salesPersonDistrictRequest)
        {
            Repository repository = new Repository(connectstring);
            await repository.RemoveSalesPersonFromDistrict(salesPersonDistrictRequest.DistrictId, salesPersonDistrictRequest.SalesPersonId);
        }
    }


    
    

}
