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
        private SalesPersonInDistrictModel _selectedSalesPerson;

        public MainViewModel()
        {
            MainWindowsLoaded = new AsyncRelayCommand(OnLoaded);
            RemoveSalesPersonCommand = new AsyncRelayCommand(RemoveSalesPerson, CanRemoveSalesPerson);
        }

        private bool CanRemoveSalesPerson()
        {
            return _selectedSalesPerson != null && !_selectedSalesPerson.Primary;
        }

        private async Task RemoveSalesPerson()
        {
            var salesPersonclient = new SalesPersonClient(baseUrl: "http://localhost:5000", new HttpClient());
            await salesPersonclient.RemoveSalesPersonFromDistrictAsync(new SalesPersonDistrictRequest()
            {
                DistrictId = SelectedDistrict.Id,
                SalesPersonId = SelectedSalesPerson.Id
            });
            var districtClient = new DistrictClient(baseUrl: "http://localhost:5000", new HttpClient());
            var districtDtos = await districtClient.GetAllDistrictsAsync();
            Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));
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

        public DistrictModel SelectedDistrict
        {
            get => _selectedDistrict;
            set
            {
                SetProperty(ref _selectedDistrict, value);
                OnPropertyChanged(nameof(SelectedDistrict));
            }
        }

        public SalesPersonInDistrictModel SelectedSalesPerson
        {
            get => _selectedSalesPerson;
            set
            {
                SetProperty(ref _selectedSalesPerson, value);
                RemoveSalesPersonCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedSalesPerson));
            }
        }

        public IAsyncRelayCommand MainWindowsLoaded { get; set; }
        public IAsyncRelayCommand RemoveSalesPersonCommand { get; set; }

        public async Task OnLoaded()
        {
            var client = new DistrictClient(baseUrl: "http://localhost:5000", new HttpClient());
            var districtDtos = await client.GetAllDistrictsAsync();
            Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));

            var salesPersonClient = new SalesPersonClient(baseUrl: "http://localhost:5000", new HttpClient());
            var salesPersonDtos = await salesPersonClient.GetAllSalesPersonsAsync();
            SalesPersons = new ObservableCollection<SalesPersonModel>(salesPersonDtos.Select(x => new SalesPersonModel(x)));
        }
    }
}
