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

        public MainViewModel()
        {

            MainWindowsLoaded = new AsyncRelayCommand(OnLoaded);
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

        public IAsyncRelayCommand MainWindowsLoaded { get; set; }

        public async Task OnLoaded()
        {
            var client = new SaleManagementClient(baseUrl: "http://localhost:5000", new HttpClient());
            var districtDtos = await client.GetAsync();
            Districts = new ObservableCollection<DistrictModel>(districtDtos.Select(x => new DistrictModel(x)));

        }
    }
}
