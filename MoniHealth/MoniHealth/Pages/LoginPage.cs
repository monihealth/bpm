using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MoniHealth.Pages;
using MoniHealth;
using System.Text.RegularExpressions;
using MoniHealth.Models;

namespace MoniHealth.Pages
{
    public class LoginPage : ContentPage
    {
        public static Entry EmailE, PasswordE;

        public GalenCloudComm Cloud = new GalenCloudComm();
        UserAccountInformation user = new UserAccountInformation();
        public LoginPage()
        {
            Label header = new Label
            {
                Text = "Welcome",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            EmailE = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            PasswordE = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 13,
                Placeholder = "Enter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button loginButton = new Button
            {
                Text = "  Login  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Button createButton = new Button
            {
                Text = "  Create Account  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            loginButton.Clicked += OnLoginBtnClicked;
            createButton.Clicked += OnCreateBtnClicked;

            // Build the page.
            Title = "MoniHealth";
            Padding = new Thickness(10, 0);
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 5,
                Children =
                {
                    new Image { Source = "MoniHealth/Resources/Images/MoniHealth_Logo.png" },
                    header,
                    EmailE,
                    PasswordE,

                    new StackLayout() //Stackception to put two buttons side by side 
                    {
                        Margin = new Thickness(20),
                        HorizontalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Horizontal,
                        Children=
                        {
                        loginButton,
                        createButton
                        }
                    },
                }
            };

            async void OnLoginBtnClicked(object sender, EventArgs e)
            {
                await GalenCloudComm.GetCloudCommunication();
                //Check Login Information Later On !
                //For now just sends to next page'
                var emailPattern = (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (EmailE.Text == null || PasswordE.Text == null)
                    LoginUnsuccessful();
                else
                    if (Regex.IsMatch(EmailE.Text, emailPattern))
                {

                    Cloud.Login(EmailE.Text, PasswordE.Text, user, )


                    Application.Current.MainPage = new TabPage();
                }
                else
                    InvalidEmail();
                // await MainPage = new NavigationPage(new PrimaryPage());
                //App.Current.MainPage = new NavigationPage();
                //await Navigation.PushAsync(new PrimaryPage());


            }

            async void OnCreateBtnClicked(object sender, EventArgs e)
            {
                await Navigation.PushAsync(new AccountCreationPage());
            }
        }
        private void LoginUnsuccessful()
        {
            DisplayAlert("Login", "Login unsuccessful: Empty email or password", "OK");
        }
        private void InvalidEmail()
        {
            DisplayAlert("Invalid email format", "Please enter a valid email address", "OK");
        }
    }
}
