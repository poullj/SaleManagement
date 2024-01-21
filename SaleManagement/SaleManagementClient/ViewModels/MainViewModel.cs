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
        private DistrictModel _selectedDistrict;
        private SalesPersonModel _selectedSalesPerson;

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
            var client = new SaleManagementClient(baseUrl: "http://localhost:5000", new HttpClient());
            await client.RemoveSalesPersonFromDistrictAsync(new SalesPersonDistrictRequest()
            {
                DistrictId = SelectedDistrict.Id,
                SalesPersonId = SelectedSalesPerson.Id
            });
            var districtDtos = await client.GetAsync();
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

        public DistrictModel SelectedDistrict
        {
            get => _selectedDistrict;
            set
            {
                SetProperty(ref _selectedDistrict, value);
                OnPropertyChanged(nameof(SelectedDistrict));
            }
        }

        public SalesPersonModel SelectedSalesPerson
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
            var client = new SaleManagementClient(baseUrl: "http://localhost:5000", new HttpClient());
            var districtDtos = await client.GetAsync();
            Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));

        }
    }
}
