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
        public override string ToString()
        {
            return Name;
        }

    }
}