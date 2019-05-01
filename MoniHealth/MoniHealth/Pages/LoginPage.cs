using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MoniHealth.Pages;
using MoniHealth;
using System.Text.RegularExpressions;

namespace MoniHealth.Pages
{
    public class LoginPage : ContentPage
    {
        public static Entry Email, Password;

        public LoginPage()
        {
            Label header = new Label
            {
                Text = "Welcome",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            Email = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 13,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            Password = new Entry
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
                    header,
                    Email,
                    Password,

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

            void OnLoginBtnClicked(object sender, EventArgs e)
            {
                //Check Login Information Later On !
                //For now just sends to next page'
                var emailPattern = (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (Email.Text == null || Password.Text == null)
                    LoginUnsuccessful();
                else
                    if (Regex.IsMatch(Email.Text, emailPattern))
                {
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
