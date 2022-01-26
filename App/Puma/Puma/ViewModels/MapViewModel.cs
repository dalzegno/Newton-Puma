using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace Puma.ViewModels
{
    public class MapViewModel
    {
        public MapViewModel()
        {
            Map = new Map();
        }

        public Map Map { get; private set; }

        
    }
}
