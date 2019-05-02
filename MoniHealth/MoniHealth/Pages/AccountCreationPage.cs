using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MoniHealth.Pages;
using MoniHealth;
using System.Text.RegularExpressions;
using MoniHealth.Models;
using System.Threading.Tasks;

namespace MoniHealth.Pages
{
    public class AccountCreationPage : ContentPage
    {
        public GalenCloudComm Cloud = new GalenCloudComm();
        UserAccountInformation user = new UserAccountInformation();

        Entry fName, lName, EmailE, Password, Confirm;

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
                Text = "Fname",
                Keyboard = Keyboard.Text,
                FontSize = 11,
                Placeholder = "Enter First Name",
                VerticalOptions = LayoutOptions.Center,
            };
            lName = new Entry
            {
                Text = "Lname",
                Keyboard = Keyboard.Email,
                FontSize = 11,
                Placeholder = "Enter Last Name",
                VerticalOptions = LayoutOptions.Center,
            };
            EmailE = new Entry
            {
                Text = "wjwinter1996@hotmail.com",
                Keyboard = Keyboard.Email,
                FontSize = 11,
                Placeholder = "Enter email address",
                VerticalOptions = LayoutOptions.Center,
            };
            Password = new Entry
            {
                Text = "TempPass123!",
                Keyboard = Keyboard.Text,
                FontSize = 11,
                Placeholder = "Enter password",
                IsPassword = true,
                VerticalOptions = LayoutOptions.Center
            };
            Confirm = new Entry
            {
                Text = "TempPass123!",
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
                    EmailE,
                    Password,
                    Confirm,
                    createButton
                }
            };

            async void OnButtonClicked(object sender, EventArgs e)
            {
                user.Email = EmailE.Text;
                user.FirstName = fName.Text;
                user.LastName = lName.Text;
                user.Password = Password.Text;
                //create an account through the galen cloud and then return to the login page
                var emailPattern = (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (EmailE.Text == null || Password.Text == null || fName.Text == null || lName.Text == null ||
                    Confirm.Text == null)
                    DisplayAlert("Error!", "Please fill out all the fields.", "OK");
                else
                    if (Password.Text != Confirm.Text)
                    DisplayAlert("Error!", "Make sure that the passwords match", "OK");
                else
                    if (Regex.IsMatch(EmailE.Text, emailPattern))
                {
                    Action success = new Action(SuccessAlert);
                    //Action<Exception> failure = new Action<Exception>(FailureAlert);
                    await GalenCloudComm.GetCloudCommunication();
                    Task T = Cloud.CreateAccount(user,success);
                    await Navigation.PopAsync();
                }
                else
                    DisplayAlert("Error!", "Please enter a valid email address", "OK");
                

                // await MainPage = new NavigationPage(new PrimaryPage());
                //App.Current.MainPage = new NavigationPage();
                //App.Current.MainPage = new NavigationPage(new PrimaryPage());
                //await Navigation.PushAsync(new PrimaryPage());
            }

            void SuccessAlert()
            {
                DisplayAlert("Account Created", "Check email to activate account", "OK");
            }
            void FailureAlert()
            {
                throw new Exception("Account Creation Failed");
            }

        }
    }
}
