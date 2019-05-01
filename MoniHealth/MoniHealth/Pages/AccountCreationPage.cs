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
    public class AccountCreationPage : ContentPage
    {
        Entry fName, lName, Email, Password, Confirm;

        public AccountCreationPage()
        {
            Label header = new Label
            {
                Text = "Create Account",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            fName = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 11,
                Placeholder = "Enter First Name",
                VerticalOptions = LayoutOptions.Center,
            };
            lName = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 11,
                Placeholder = "Enter Last Name",
                VerticalOptions = LayoutOptions.Center,
            };
            Email = new Entry
            {
                Keyboard = Keyboard.Email,
                FontSize = 11,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.Center,
            };
            Password = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 11,
                Placeholder = "Enter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.Center
            };
            Confirm = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 11,
                Placeholder = "ReEnter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.Center
            };

            Button createButton = new Button
            {
                Text = "  Create Account  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
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

            void OnButtonClicked(object sender, EventArgs e)
            {
                //create an account through the galen cloud and then return to the login page
                var emailPattern = (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (Email.Text == null || Password.Text == null || fName.Text == null || lName.Text == null ||
                    Confirm.Text == null)
                    DisplayAlert("Error!", "Please fill out all the fields.", "OK");
                else
                    if (Password.Text != Confirm.Text)
                    DisplayAlert("Error!", "Make sure that the passwords match", "OK");
                else
                    if (Regex.IsMatch(Email.Text, emailPattern))
                    Navigation.PopAsync();
                else
                    DisplayAlert("Error!", "Please enter a valid email address", "OK");
                

                // await MainPage = new NavigationPage(new PrimaryPage());
                //App.Current.MainPage = new NavigationPage();
                //App.Current.MainPage = new NavigationPage(new PrimaryPage());
                //await Navigation.PushAsync(new PrimaryPage());
            }




        }
    }
}
