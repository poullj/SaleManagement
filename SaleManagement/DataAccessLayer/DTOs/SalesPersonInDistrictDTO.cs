namespace DataAccessLayer.DTOs
{
    public class SalesPersonInDistrictDTO : SalesPersonDTO
    {
        public bool Primary { get; set; }
        public bool Secondary { get; set; }

    }
}