﻿using System;
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
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Settings Page" }
                }
            };
        }
    }
}