using DataAccessLayer;
using DataAccessLayer.DTOs;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Request;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistrictController : ControllerBase
    {
        private Repository _repository { get; set; }
        
        public DistrictController(IConfiguration configuration)
        {
            string connectstring = configuration.GetConnectionString("DefaultConnection");
            _repository = new Repository(connectstring);
        }
               


        [HttpGet(Name = "GetAllDistricts")]
        public async Task<IEnumerable<DistrictDTO>> GetAllDistricts()
        {
            return await _repository.GetAllDistricts();
        }
    }
}
