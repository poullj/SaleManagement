namespace DataAccessLayer.DatabaseClasses
{
    public class DistrictSalesPerson
    {
        public int SalesPersonId { get; set; }
        public int DistrictId { get; set; }
        public bool Secondary { get; set; }
    }
}
