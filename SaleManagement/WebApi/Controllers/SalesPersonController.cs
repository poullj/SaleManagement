using DataAccessLayer;
using DataAccessLayer.DTOs;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Request;

namespace WebApi.Controllers
{
    

    [ApiController]
    [Route("[controller]")]
    public class SalesPersonController : ControllerBase
    {
        string connectstring { get; set; } = "Data Source=localhost;Initial Catalog=SaleManagement;Integrated Security=True;Encrypt=optional;";

        private readonly ILogger<SalesPersonController> _logger;
        
        public SalesPersonController(ILogger<SalesPersonController> logger)
        {
            _logger = logger;
        }

       
        [HttpGet(Name = "GetAllSalesPersons")]
        public async Task<IEnumerable<SalesPersonDTO>> GetAllSalesPersons()
        {
            Repository repository = new Repository(connectstring);
            return await repository.GetAllSalesPersons();
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
