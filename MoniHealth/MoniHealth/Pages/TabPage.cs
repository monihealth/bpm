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
            var navigationPage = new NavigationPage(new GraphsPage());
            //navigationPage.Icon = "something.png";
            Padding = new Thickness(10, 0);
            navigationPage.Title = "MoniHealth";

            Children.Add(new MainPage());
            Children.Add(navigationPage);
            Children.Add(new SettingsPage());
            Children.Add(new SimpleCirclePage());

        }
    }
}