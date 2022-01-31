using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Puma.Models;
using Puma.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms.Xaml;
using System.Linq;
using Puma.Views;

namespace Puma.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    class PoiViewModel : BaseViewModel
    {
        readonly IPoiService _poiService;
        readonly IDialogService _dialogService;
        public PoiViewModel(IPoiService poiService, IDialogService dialogService)
        {
            _poiService = poiService;
            _dialogService = dialogService;
            _tags = new List<Tag>();
            _tagButtons = new ObservableCollection<Tag>();
            GetTagsFromDb();
            //RemoveTagCommand = new Command<Tag>(RemoveTag);
        }

        Command _createPoiCommand;
        Command _removeTagCommand;
        Command _poiCollectionPopupCommand;
        Command _poiCreationPopupCommand;
        Command _selectedPoiCommand;

        public Command SelectedPoiCommand => _selectedPoiCommand ?? (_selectedPoiCommand = new Command(SelectPoi));

        public Command CreatePoiCommand => _createPoiCommand ?? (_createPoiCommand = new Command(CreatePoi, CanCreate));
        public Command RemoveTagCommand => _removeTagCommand ?? (_removeTagCommand = new Command(RemoveTag));

        public Command PoiCollectionPopupCommand => _poiCollectionPopupCommand ?? (_poiCollectionPopupCommand = new Command(PoiCollectionPopup));
        public Command PoiCreationPopupCommand => _poiCreationPopupCommand ?? (_poiCreationPopupCommand = new Command(PoiCreationPopup));

        public string _name;
        public string _description;
        public string _country { get; set; } = "Click or search the map";
        public string _area { get; set; } = "to start exploring!";
        public string _streetName;
        public string _longitude;
        public string _latitude;
        public PointOfInterest _selectedSinglePoi;

        public bool openPoiCreationBool { get; set; } = false;
        public bool openPoiCollectionBool { get; set; } = false;
        public bool poiCollectionVisibleBool { get; set; } = false;
        public bool poiSingleVisibleBool { get; set; } = false;
        public async void GetTagsFromDb()
        {

            Tags = await _poiService.GetTags();
        }

        // Poi Collection
        public ObservableCollection<PointOfInterest> _poiCollection;
        public ObservableCollection<PointOfInterest> PoiCollection
        {
            get => _poiCollection;
            set
            {
                _poiCollection = value;
                OnPropertyChanged(nameof(PoiCollection));
            }
        }
        public PointOfInterest SelectedSinglePoi
        {
            get => _selectedSinglePoi;
            set
            {
                _selectedSinglePoi = value;
                OnPropertyChanged(nameof(SelectedSinglePoi));
            }
        }
            

        // TAGS
        public List<Tag> _tags;
        public List<Tag> Tags
        {
            get => _tags;
            set
            {
                 _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public ObservableCollection<Tag> _tagButtons;
        public ObservableCollection<Tag> TagButtons
        {
            get => _tagButtons;
            set
            {
                _tagButtons = value;
                OnPropertyChanged(nameof(TagButtons));
            }
        }
        public Tag _tag;
        public Tag SelectedTagInfo
        {
            get => _tag;
            set
            {
                _tag = value;
                TagButtons.Add(value);
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(TagButtons));
               
            }
        }

        

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
                TagIds = TagButtons.Count > 0 ? TagButtons.Select(x => x.Id).ToList() : new List<int>()
            };

            var createdPoi = await _poiService.CreatePoiAsync(poi);
            if (createdPoi != null)
            {
                await _dialogService.ShowMessageAsync("Welcome!", $"Welcome to PUMA \"{createdPoi.Name}\".");
                return;
            }
        }

        private void RemoveTag(object tag)
        {
            TagButtons.Remove((Tag)tag);
        }
        private async void PoiCollectionPopup()
        {
            openPoiCollectionBool = !openPoiCollectionBool;
            openPoiCreationBool = false;
            poiCollectionVisibleBool = true;
            poiSingleVisibleBool = false;
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));
            OnPropertyChanged(nameof(poiCollectionPopupState));
            OnPropertyChanged(nameof(PoiCreationPopupState));
            PoiCollection = await _poiService.GetAllAsync();
           
        }
        private void PoiCreationPopup()
        {
            openPoiCreationBool = !openPoiCreationBool;
            openPoiCollectionBool = false;
            OnPropertyChanged(nameof(PoiCreationPopupState));
            OnPropertyChanged(nameof(poiCollectionPopupState));
        }

        private void SelectPoi()
        {
            poiCollectionVisibleBool = !poiCollectionVisibleBool;
            poiSingleVisibleBool = !poiSingleVisibleBool;
            if (poiSingleVisibleBool == true)
                MainPage.Instance.GoToLocation(SelectedSinglePoi);
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));

        }

        public void SetAddress(string address)
        {
            var words = address?.Split('\n') ?? Array.Empty<string>();
            if (address == null || address == "")
            {
                StreetName = "";
                Area = "No location found";
                Country = "";
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            else if (words.Length == 2)
            {
                Area = words[0];
                Country = words[1];
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            else if (words.Length == 3)
            {
                StreetName = words[0];
                Area = words[1];
                Country = words[2];
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            
        }

        public bool PoiCreationPopupState => openPoiCreationBool;
        public bool poiCollectionPopupState => openPoiCollectionBool;
        public bool PoiCollectionVisible => poiCollectionVisibleBool;
        public bool PoiSingleVisible => poiSingleVisibleBool;
    }
}
