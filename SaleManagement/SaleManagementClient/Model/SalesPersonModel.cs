using CommunityToolkit.Mvvm.ComponentModel;
using WebApi.Client;

namespace SaleManagementWpfClient.Model
{
    public class SalesPersonModel : ObservableObject
    {
        SalesPersonDTO _salesPersonDTO;
        public SalesPersonModel(SalesPersonDTO salesPersonDTO)
        {
            SalesPersonDTO = salesPersonDTO;
        }

        public SalesPersonDTO SalesPersonDTO { get => _salesPersonDTO; set => _salesPersonDTO = value; }

        public int Id
        {
            get => _salesPersonDTO.Id;
        }

        public string Name
        {
            get => _salesPersonDTO.Name;
            set
            {
                _salesPersonDTO.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

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