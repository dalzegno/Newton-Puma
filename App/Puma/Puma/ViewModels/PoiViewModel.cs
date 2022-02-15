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
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using System.Globalization;
using Puma.Enums;

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
        }

        Command _createPoiCommand;
        Command _removeTagCommand;
        Command _poiGetCollectionCommand;
        Command _poiSingleViewCommand;
        Command _poiCollectionViewCommand;
        Command _addCommentCommand;
        Command _addGradeLikeCommand;
        Command _addGradeDislikeCommand;
        Command _removeCommentCommand;
        public Command CreatePoiCommand => _createPoiCommand ?? (_createPoiCommand = new Command(CreatePoi, CanCreate));
        public Command RemoveTagCommand => _removeTagCommand ?? (_removeTagCommand = new Command(RemoveTag));

        public Command PoiGetCollectionCommand => _poiGetCollectionCommand ?? (_poiGetCollectionCommand = new Command(PoiGetCollection));
        public Command PoiSingleViewCommand => _poiSingleViewCommand ?? (_poiSingleViewCommand = new Command(PoiSinglePopup));
        public Command PoiCollectionViewCommand => _poiCollectionViewCommand ?? (_poiCollectionViewCommand = new Command(PoisPopup));
        public Command AddCommentCommand => _addCommentCommand ?? (_addCommentCommand = new Command(AddComment));

        public Command AddGradeLikeCommand => _addGradeLikeCommand ?? (_addGradeLikeCommand = new Command(AddGradeLike));
        public Command AddGradeDislikeCommand => _addGradeDislikeCommand ?? (_addGradeDislikeCommand = new Command(AddGradeDislike));
        public Command RemoveCommentCommand => _removeCommentCommand ?? (_removeCommentCommand = new Command(RemoveComment));

        public string _name;
        public string _description;
        public string _country { get; set; } = "Click or search the map";
        public string _area { get; set; } = "to start exploring!";
        public string _streetName;
        public string _longitude;
        public string _latitude;

        public PointOfInterest _selectedSinglePoi;
        public ObservableCollection<Comment> _commentCollection;
        public string _likeCount;
        public string _dislikeCount;
        public string _commentBody;


        public bool openPoiCreationBool { get; set; } = false;
        public bool openPoiCollectionBool { get; set; } = false;
        public bool poiCollectionVisibleBool { get; set; } = true;
        public bool poiSingleVisibleBool { get; set; } = false;
        public bool poiCommentPostVisible { get; set; } = false;
        //public bool poiCommentRemoveVisible { get; set; } = false;
        private bool isAddPoiVisible { get; set; } = false;



        #region Fields
        public string LikeCount
        {
            get => _likeCount;
            set
            {
                _likeCount = value;
                OnPropertyChanged(nameof(LikeCount));
            }
        }
        public string DislikeCount
        {
            get => _dislikeCount;
            set
            {
                _dislikeCount = value;
                OnPropertyChanged(nameof(DislikeCount));
            }
        }
        public bool IsAddPoiVisible
        {
            get => isAddPoiVisible;
            set
            {
                isAddPoiVisible = value;
                OnPropertyChanged(nameof(IsAddPoiVisible));
            }
        }
        public bool PoiCommentPostVisible
        {
            get => poiCommentPostVisible;
            set
            {
                poiCommentPostVisible = value;
                OnPropertyChanged(nameof(PoiCommentPostVisible));
            }
        }
        //public bool PoiCommentRemoveVisible
        //{
        //    get => poiCommentRemoveVisible;
        //    set
        //    {
        //        poiCommentRemoveVisible = value;
        //        OnPropertyChanged(nameof(PoiCommentRemoveVisible));
        //    }
        //}
        public string CommentBody
        {
            get => _commentBody;
            set
            {
                _commentBody = value;
                OnPropertyChanged(nameof(CommentBody));
            }
        }

        public ObservableCollection<Comment> CommentCollection
        {
            get => _commentCollection;
            set
            {
                _commentCollection = value;
                OnPropertyChanged(nameof(CommentCollection));

            }
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

        #endregion

        private bool CanCreate() => !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrWhiteSpace(Name) && App.LoggedInUser != null;

        private void PoiGetCollection()
        {
            poiCollectionVisibleBool = true;
            poiSingleVisibleBool = false;
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));
            OnPropertyChanged(nameof(PoiCollection));
        }

        public async void PoisPopup()
        {
            SelectedSinglePoi = null;
            OnPropertyChanged(nameof(SelectedSinglePoi));
            poiCollectionVisibleBool = true;
            poiSingleVisibleBool = false;
            PoiCollection = await GetPoisFromDb(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude));
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));
        }
        public void PoiSinglePopup()
        {


            if (SelectedSinglePoi != null)
                MainPage.Instance.GoToLocation(SelectedSinglePoi, 1);

            poiCollectionVisibleBool = false;
            OnPropertyChanged(nameof(PoiCollectionVisible));
            poiSingleVisibleBool = true;
            OnPropertyChanged(nameof(PoiSingleVisible));

            if (SelectedSinglePoi == null)
                return;

            var a = SelectedSinglePoi.Address;
            StreetName = a.StreetName;
            Area = a.Area;
            Country = a.Country;
            MainPage.Instance.WeatherViewModel1.SetWeather(SelectedSinglePoi.Position.Latitude, SelectedSinglePoi.Position.Longitude);
            OnPropertyChanged(nameof(StreetName));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Country));
        }

        public bool PoiCreationPopupState => openPoiCreationBool;
        public bool poiCollectionPopupState => openPoiCollectionBool;
        public bool PoiCollectionVisible => poiCollectionVisibleBool;
        public bool PoiSingleVisible => poiSingleVisibleBool;

        private async void AddGradeLike(object poi)
        {
            if (App.LoggedInUser == null)
                return;
            if (poi == null)
                return;
            var x = (Grid)poi;
            PointOfInterest selectedPoi = x.BindingContext as PointOfInterest;
            int gradeType = (int)GradeType.Liked;
            AddGradeDto grade = new AddGradeDto
            {
                PoiId = selectedPoi.Id,
                UserId = App.LoggedInUser.Id,
                Grade = gradeType
            };
            await _poiService.AddGradeAsync(grade);
            await SetLatAndLon(Convert.ToDouble(Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(Longitude, CultureInfo.InvariantCulture));

        }
        private async void AddGradeDislike(object poi)
        {
            if (App.LoggedInUser == null)
                return;
            if (poi == null)
                return;
            var x = (Grid)poi;
            PointOfInterest selectedPoi = x.BindingContext as PointOfInterest;
            int gradeType = (int)GradeType.Disliked;
            AddGradeDto grade = new AddGradeDto
            {
                PoiId = selectedPoi.Id,
                UserId = App.LoggedInUser.Id,
                Grade = gradeType
            };
            await _poiService.AddGradeAsync(grade);
            await SetLatAndLon(Convert.ToDouble(Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(Longitude, CultureInfo.InvariantCulture));
        }
        private async void AddComment()
        {
            if (App.LoggedInUser == null || string.IsNullOrWhiteSpace(CommentBody))
                return;
            AddCommentDto comment = new AddCommentDto
            {
                UserId = App.LoggedInUser.Id,
                PointOfInterestId = SelectedSinglePoi.Id,
                Body = CommentBody
            };

            var poi = await _poiService.AddCommentAsync(comment);
            SelectedSinglePoi = poi;
            CommentBody = "";
            OnPropertyChanged(nameof(SelectedSinglePoi));
            OnPropertyChanged(nameof(CommentCollection));

        }
        private async void RemoveComment(object idObj)
        {
            var answer = await _dialogService.ShowYesNoActionSheet("Are you sure you want to delete this comment?");

            if (answer == null || answer == "No")
                return;

            var id = (int)idObj;
            var commentToRemove = SelectedSinglePoi.Comments.FirstOrDefault(c => c.Id == id);
            var removedComment = await _poiService.DeleteComment(App.LoggedInUser.Id, commentToRemove.Id);

            var selectedSingleId = SelectedSinglePoi.Id;
            if (removedComment != null)
            {
                await SetLatAndLon(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude));
                SelectedSinglePoi = PoiCollection.FirstOrDefault(poi => poi.Id == selectedSingleId);
                await _dialogService.ShowMessageAsync("Removed", "Comment removed");
            }

        }
        private async void GetTagsFromDb()
        {
            Tags = await _poiService.GetTags();
        }
        private async Task<ObservableCollection<PointOfInterest>> GetPoisFromDb(double lat, double lon)
        {
            var pois = await _poiService.GetAsync(lat, lon);

            return pois != null ? new ObservableCollection<PointOfInterest>(pois) : null;
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
            else if (words.Length == 1)
            {
                StreetName = "";
                Area = "";
                Country = words[0];
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            else if (words.Length == 2)
            {
                StreetName = "";
                Area = words[0];
                Country = words[1];
                OnPropertyChanged(nameof(StreetName));
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
        public async Task SetLatAndLon(double lat, double lon)
        {
            if (lat == 0 || lon == 0)
            {
                return;
            }

            Latitude = lat.ToString();
            Longitude = lon.ToString();

            PoiCollection = await GetPoisFromDb(lat, lon);

            if (PoiCollection == null || PoiCollection.Count < 1)
                return;

            foreach (var poi in PoiCollection)
            {
                var pin = MainPage.Instance.CreatePin(poi);
                MainPage.Instance.AddPin(pin);
            }
        }

        // Name, Description, Position, Address, TagIds

        private async void CreatePoi()
        {
            var poi = new AddPoiDto()
            {
                UserId = App.LoggedInUser.Id,
                Name = Name,
                Description = Description,
                Position = new PositionPoi(Convert.ToDouble(Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(Longitude, CultureInfo.InvariantCulture)),
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
                await _dialogService.ShowMessageAsync("Created", $"Added a new POI");
                ClearFields();

                return;
            }
        }

        private void ClearFields()
        {
            Name = "";
            Description = "";
            TagButtons.Clear();
        }

        private void RemoveTag(object tag)
        {
            if (tag != null)
                TagButtons.Remove((Tag)tag);
        }
    }
}