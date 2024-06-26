﻿using CommunityToolkit.Mvvm.ComponentModel;
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
            _salesPersons = new ObservableCollection<SalesPersonInDistrictModel>(DistrictDTO.SalesPersons.Select(x=>new SalesPersonInDistrictModel(x)));
            _stores = new ObservableCollection<StoreModel>(DistrictDTO.Stores.Select(x => new StoreModel(x)));
        }

        private DistrictDTO _districtDTO;

        private DistrictDTO DistrictDTO { get => _districtDTO; set => _districtDTO = value; }

        public int Id
        {
            get => _districtDTO.Id;
            set
            {
                _districtDTO.Id = Id;
                OnPropertyChanged(nameof(Id));
            }
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

        ObservableCollection<SalesPersonInDistrictModel> _salesPersons;
        public ObservableCollection<SalesPersonInDistrictModel> SalesPersons
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

        public override string ToString()
        {
            return Name;
        }
    }
}
