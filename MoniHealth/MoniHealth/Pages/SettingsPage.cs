using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MoniHealth.Pages
{
    public class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            Title = "Settings";
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Settings Page" }
                }
            };

            


        }
    }
}