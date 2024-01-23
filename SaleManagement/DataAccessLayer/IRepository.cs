using DataAccessLayer.DTOs;

namespace DataAccessLayer
{
    public interface IRepository
    {
        Task AddSalesPersonToDistrict(int districtId, int salesPersonID, bool primary = false, bool secondary = false);
        Task<List<DistrictDTO>> GetAllDistricts();
        Task<List<SalesPersonDTO>> GetAllSalesPersons();
        Task RemoveSalesPersonFromDistrict(int districtId, int salesPersonID);
    }
}