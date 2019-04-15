using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MoniHealth.Pages
{
	public class TabPage : TabbedPage
	{

		public TabPage ()
        {
            var navigationPage = new NavigationPage();
            //navigationPage.Icon = "something.png";
            Padding = new Thickness(10, 0);
            

            Children.Add(new MainPage());
            Children.Add(new GraphsPage());
            Children.Add(new SettingsPage());
            //Children.Add(new SimpleCirclePage());
            NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
        }
    }
}