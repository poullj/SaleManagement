using CommunityToolkit.Mvvm.ComponentModel;
using WebApi.Client;

namespace SaleManagementWpfClient.Model
{
    public class SalesPersonInDistrictModel : SalesPersonModel
    {
        SalesPersonInDistrictDTO _salesPersonDTO;
        public SalesPersonInDistrictModel(SalesPersonInDistrictDTO salesPersonDTO) : base(salesPersonDTO) 
        {
            SalesPersonDTO = salesPersonDTO;
        }

        public SalesPersonInDistrictDTO SalesPersonDTO { get => _salesPersonDTO; set => _salesPersonDTO = value; }

        
        public bool Primary
        {
            get => _salesPersonDTO.Primary;
            set
            {
                _salesPersonDTO.Primary = value;
                OnPropertyChanged(nameof(Primary));
            }
        }

        public bool Secondary
        {
            get => _salesPersonDTO.Secondary;
            set
            {
                _salesPersonDTO.Secondary = value;
                OnPropertyChanged(nameof(Secondary));
            }
        }
    }
}