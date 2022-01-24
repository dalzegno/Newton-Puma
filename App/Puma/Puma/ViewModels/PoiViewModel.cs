using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Puma.Models;
using Puma.Services;

namespace Puma.ViewModels
{
    class PoiViewModel : BaseViewModel
    {

        readonly IPoiService _poiService;
        readonly IDialogService _dialogService;
        public PoiViewModel(IPoiService poiService, IDialogService dialogService)
        {
            _poiService = poiService;
            _dialogService = dialogService;
        }

        Command _createPoiCommand;
        public Command CreatePoiCommand => _createPoiCommand ?? (_createPoiCommand = new Command(CreatePoi, CanCreate));

        public string _name;
        public string _description;
        public string _country;
        public string _area;
        public string _streetName;
        public string _longitude;
        public string _latitude;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(Name);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(Description);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged(Country);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Area
        {
            get => _area;
            set
            {
                _area = value;
                OnPropertyChanged(Area);
                CreatePoiCommand.ChangeCanExecute();
            }
        }

        public string StreetName
        {
            get => _streetName;
            set
            {
                _streetName = value;
                OnPropertyChanged(StreetName);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged(Latitude);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged(Longitude);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        private bool CanCreate() => !string.IsNullOrWhiteSpace(Description)  && !string.IsNullOrWhiteSpace(Name);
        private async void CreatePoi()
        {
            var poi = new AddPoiDto()
            {
                Name = Name,
                Description = Description,
                Position = new PositionPoi
                {
                    Latitude = Convert.ToDouble(Latitude),
                    Longitude = Convert.ToDouble(Longitude)
                },
                Address = new Address
                {
                    StreetName = StreetName,
                    Area = Area,
                    Country = Country
                },
                TagIds = new List<int>() { 1, 2, 3 }


            };

            var createdPoi = await _poiService.CreatePoiAsync(poi);

            if (createdPoi != null)
            {
                await _dialogService.ShowMessageAsync("Welcome!", $"Welcome to PUMA \"{createdPoi.Name}\".");
                return;
            }
        }

    }

    
}
