using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoniHealth.Pages;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MoniHealth
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=77f7c402-c578-48bb-b3cb-d94b40cda896;" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
