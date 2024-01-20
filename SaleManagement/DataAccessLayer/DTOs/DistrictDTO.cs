using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTOs
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public List<SalesPersonDTO> SalesPersons { get; set; } = [];
        public List<StoreDTO> Stores { get; set; } = [];
    }
}
