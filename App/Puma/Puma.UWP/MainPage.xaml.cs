using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Puma.UWP
{
    public sealed partial class MainPage
    {
        public string MapServiceToken { get; set; } = "TeY7G1WAvjb34E4Whm3O~rf_C1-AnJK93MZy_MauxdQ~AiFZAkkXvdlx_N3mXTm1W3_cIDvh1thBdovOaZHGxUf_a28xHDYE9403N7OJQX_v";

        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init(MapServiceToken);
            LoadApplication(new Puma.App());
        }
    }
}
