using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MoniHealth.Pages;
using MoniHealth;

namespace MoniHealth.Pages
{
    public class AccountCreationPage : ContentPage
    {
        Entry fName, lName, Email, Password, Confirm;

        public AccountCreationPage()
        {
            Label header = new Label
            {
                Text = "Login",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            fName = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter First Name",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            lName = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 10,
                Placeholder = "Enter Last Name",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            Email = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 10,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            Password = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Confirm = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "ReEnter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button createButton = new Button
            {
                Text = "Create Account",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            createButton.Clicked += OnButtonClicked;

            // Build the page.
            Title = "MoniHealth";
            //Padding = new Thickness(10, 0);
            Content = new StackLayout
            {
                Spacing = 7,
                Children =
                {
                    header,
                    fName,
                    lName,
                    Email,
                    Password,
                    Confirm,
                    createButton
                }
            };

            async void OnButtonClicked(object sender, EventArgs e)
            {
                //create an account through the galen cloud and then return to the login page

                await Navigation.PopAsync();

                // await MainPage = new NavigationPage(new PrimaryPage());
                //App.Current.MainPage = new NavigationPage();
                //App.Current.MainPage = new NavigationPage(new PrimaryPage());
                //await Navigation.PushAsync(new PrimaryPage());
            }




        }
    }
}