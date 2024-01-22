using DataAccessLayer;
using DataAccessLayer.DTOs;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Request;

namespace WebApi.Controllers
{
    

    [ApiController]
    [Route("[controller]")]
    public class SalesPersonController : ControllerBase
    {

        private Repository _repository { get; set; }


        public SalesPersonController(IConfiguration configuration)
        {
            string connectstring = configuration.GetConnectionString("DefaultConnection");
            _repository = new Repository(connectstring);
        }

       
        [HttpGet(Name = "GetAllSalesPersons")]
        public async Task<IEnumerable<SalesPersonDTO>> GetAllSalesPersons()
        {
            return await _repository.GetAllSalesPersons();
        }

        [HttpPut("AddSalesPersonToDistrict")]
        public async Task AddSalesPersonToDistrict([FromBody] SalesPersonRolesDistrictRequest salesPersonRolesDistrictRequest)
        {
            await _repository.AddSalesPersonToDistrict(salesPersonRolesDistrictRequest.DistrictId,
                                                      salesPersonRolesDistrictRequest.SalesPersonId,
                                                      salesPersonRolesDistrictRequest.Primary,
                                                      salesPersonRolesDistrictRequest.Secondary);
        }

        [HttpPut("RemoveSalesPersonFromDistrict")]
        public async Task RemoveSalesPersonFromDistrict([FromBody] SalesPersonDistrictRequest salesPersonDistrictRequest)
        {
            await _repository.RemoveSalesPersonFromDistrict(salesPersonDistrictRequest.DistrictId, salesPersonDistrictRequest.SalesPersonId);
        }
    }


    
    

}
