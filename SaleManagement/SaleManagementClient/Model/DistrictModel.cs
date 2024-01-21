using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Client;

namespace SaleManagementWpfClient.Model
{
    public class DistrictModel : ObservableObject
    {
        public DistrictModel(DistrictDTO districtDTO)
        {
            DistrictDTO = districtDTO;
            _salesPersons = new ObservableCollection<SalesPersonModel>(DistrictDTO.SalesPersons.Select(x=>new SalesPersonModel(x)));
            _stores = new ObservableCollection<StoreModel>(DistrictDTO.Stores.Select(x => new StoreModel(x)));
        }

        private DistrictDTO _districtDTO;

        public DistrictDTO DistrictDTO { get => _districtDTO; set => _districtDTO = value; }

        public int Id
        {
            get => _districtDTO.Id;
        }

        public string Name 
        {
            get => _districtDTO.Name;
            set
            {
                _districtDTO.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        ObservableCollection<SalesPersonModel> _salesPersons;
        public ObservableCollection<SalesPersonModel> SalesPersons
        {
            get => _salesPersons;
            set
            {
                SetProperty(ref _salesPersons, value);
                OnPropertyChanged(nameof(SalesPersons));
            }
        }

        ObservableCollection<StoreModel> _stores;
        public ObservableCollection<StoreModel> Stores
        {
            get => _stores;
            set
            {
                SetProperty(ref _stores, value);
                OnPropertyChanged(nameof(Stores));
            }
        }
    }
}
