namespace WebApi.Request
{
    public class SalesPersonRolesDistrictRequest : SalesPersonDistrictRequest
    {
        public bool Primary { get; set; }
        public bool Secondary { get; set; }
    }
}
