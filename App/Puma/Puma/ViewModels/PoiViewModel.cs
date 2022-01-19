using System;
using System.Collections.Generic;
using System.Text;

namespace Puma.ViewModels
{
    class PoiViewModel : BaseViewModel
    {
        public PoiViewModel()
        {

        }

        public string _longitude;
        public string _latitude;

        public string Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged(Longitude);
            }
        }
        public string Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged(Latitude);
            }
        }
    }

    
}
