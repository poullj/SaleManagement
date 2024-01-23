using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaleManagementWpfClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Client;

namespace SaleManagementWpfClient.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<DistrictModel> _districts;
        private ObservableCollection<SalesPersonModel> _salesPersons;
        private DistrictModel _selectedDistrict;
        private SalesPersonInDistrictModel _selectedSalesPersonInDistrict;
        private SalesPersonModel _selectedSalesPerson;
        private IDistrictClient _districtClient;
        private ISalesPersonClient _salesPersonClient;

        public MainViewModel(IDistrictClient districtClient, ISalesPersonClient salesPersonClient)
        {
            MainWindowsLoaded = new AsyncRelayCommand(OnLoaded);
            RemoveSalesPersonCommand = new AsyncRelayCommand(RemoveSelectedSalesPersonInDistrict, CanRemoveSalesPerson);
            AddSalesPersonCommand = new AsyncRelayCommand(AddSelectedSalesPerson, CanAddSalesPerson);
            _districtClient = districtClient;
            _salesPersonClient = salesPersonClient;
        }

        private bool CanAddSalesPerson()
        {
            return _selectedSalesPerson != null && _selectedDistrict != null && ! _selectedDistrict.SalesPersons.Any(x=>x.Id == _selectedSalesPerson.Id);
        }

        private async Task AddSelectedSalesPerson()
        {
            if (SelectedDistrict != null && SelectedSalesPerson != null)
            {
                var selectedDistrictId = SelectedDistrict.Id;
                
                await _salesPersonClient.AddSalesPersonToDistrictAsync(new SalesPersonRolesDistrictRequest()
                {
                    DistrictId = SelectedDistrict.Id,
                    SalesPersonId = SelectedSalesPerson.Id,
                    Primary = Primary,
                    Secondary = Secondary,
                });
                
                var districtDtos = await _districtClient.GetAllDistrictsAsync();
                Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));
                SelectedDistrict = Districts.Where(x => x.Id == selectedDistrictId).SingleOrDefault();
                SelectedSalesPersonInDistrict = SelectedDistrict?.SalesPersons.Where(x => x.Id == SelectedSalesPerson.Id).SingleOrDefault();
            }
        }

        private bool CanRemoveSalesPerson()
        {
            return _selectedSalesPersonInDistrict != null && !_selectedSalesPersonInDistrict.Primary;
        }

        private async Task RemoveSelectedSalesPersonInDistrict()
        {
            if (SelectedDistrict != null && SelectedSalesPersonInDistrict != null)
            {
                var selectedDistrictId = SelectedDistrict.Id;
                var selectedSalesPersonInDistrictId = SelectedSalesPersonInDistrict.Id;
                
                await _salesPersonClient.RemoveSalesPersonFromDistrictAsync(new SalesPersonDistrictRequest()
                {
                    DistrictId = SelectedDistrict.Id,
                    SalesPersonId = SelectedSalesPersonInDistrict.Id
                });
                
                var districtDtos = await _districtClient.GetAllDistrictsAsync();
                Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));
                SelectedDistrict = Districts.Where(x => x.Id == selectedDistrictId).SingleOrDefault();
                SelectedSalesPersonInDistrict = SelectedDistrict?.SalesPersons.Where(x => x.Id == selectedSalesPersonInDistrictId).SingleOrDefault();
            }
        }

        public ObservableCollection<DistrictModel> Districts 
        { 
            get => _districts;
            set
            {
                SetProperty(ref _districts, value);
                OnPropertyChanged(nameof(Districts));
            }
        }

        public ObservableCollection<SalesPersonModel> SalesPersons
        {
            get => _salesPersons;
            set
            {
                SetProperty(ref _salesPersons, value);
                OnPropertyChanged(nameof(SalesPersons));
            }
        }
        
        private bool _primary;
        public bool Primary
        {
            get => _primary;
            set
            {
                SetProperty(ref _primary, value);
                OnPropertyChanged(nameof(Primary));
            }
        }

        private bool _secondary;
        public bool Secondary
        {
            get => _secondary;
            set
            {
                SetProperty(ref _secondary, value);
                OnPropertyChanged(nameof(Secondary));
            }
        }


        public DistrictModel SelectedDistrict
        {
            get => _selectedDistrict;
            set
            {
                SetProperty(ref _selectedDistrict, value);
                AddSalesPersonCommand.NotifyCanExecuteChanged();
                RemoveSalesPersonCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedDistrict));
            }
        }

        public SalesPersonInDistrictModel SelectedSalesPersonInDistrict
        {
            get => _selectedSalesPersonInDistrict;
            set
            {
                SetProperty(ref _selectedSalesPersonInDistrict, value);
                RemoveSalesPersonCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedSalesPersonInDistrict));
            }
        }

        public SalesPersonModel SelectedSalesPerson
        {
            get => _selectedSalesPerson;
            set
            {
                SetProperty(ref _selectedSalesPerson, value);
                AddSalesPersonCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedSalesPerson));
            }
        }

        public IAsyncRelayCommand MainWindowsLoaded { get; set; }
        public IAsyncRelayCommand RemoveSalesPersonCommand { get; set; }

        public IAsyncRelayCommand AddSalesPersonCommand { get; set; }

        public async Task OnLoaded()
        {
            var districtDtos = await _districtClient.GetAllDistrictsAsync();
            Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));
            SelectedDistrict = Districts.First();

            
            var salesPersonDtos = await _salesPersonClient.GetAllSalesPersonsAsync();
            SalesPersons = new ObservableCollection<SalesPersonModel>(salesPersonDtos.Select(x => new SalesPersonModel(x)));
            
        }
    }
}
