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
        string connectstring { get; set; } = "Data Source=localhost;Initial Catalog=SaleManagement;Integrated Security=True;Encrypt=optional;";

        private readonly ILogger<DistrictController> _logger;
        
        public DistrictController(ILogger<DistrictController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllDistricts")]
        public async Task<IEnumerable<DistrictDTO>> GetAllDistricts()
        {
            Repository repository = new Repository(connectstring);
            return await repository.GetAllDistricts();
        }
    }
}
