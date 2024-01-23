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
    public class StoreModel : ObservableObject
    {
        public StoreModel(StoreDTO storeDTO)
        {
            StoreDTO = storeDTO;
        }

        private StoreDTO _storeDTO;

        private StoreDTO StoreDTO { get => _storeDTO; set => _storeDTO = value; }

        public int Id
        {
            get => _storeDTO.Id;
        }

        public string Name 
        {
            get => _storeDTO.Name;
            set
            {
                _storeDTO.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
