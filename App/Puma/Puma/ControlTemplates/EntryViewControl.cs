using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Puma.ControlTemplates
{
    public class EntryViewControl : ContentView
    {
        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(EntryViewControl), string.Empty);
        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(EntryViewControl), string.Empty);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(PlaceholderText), typeof(bool), typeof(EntryViewControl), false);
        
        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }
        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set { 
                SetValue(EntryTextProperty, value);
            }
            
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }
    }
}
