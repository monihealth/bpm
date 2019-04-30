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



            Button Logout = new Button
            {
                Text = "  Logout  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            Logout.Clicked += LogoutCha;

            Title = "Settings";
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Settings Page" },
                    Logout
                }
            };

            


        }

        void LogoutCha(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }


    }
}