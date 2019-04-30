using Microcharts.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MoniHealth.Pages
{
    public class MainPage : ContentPage
    {
        

        public MainPage()
        {
            Title = "Moni Health";
            //BPMRecords record = new BPMRecords();
            GraphsPage gif = new GraphsPage();
            Object[] Last = gif.LastRecord();

            ChartView minichart = new ChartView();
            minichart = gif.MainChart;

            var Lastest = new Label
            {
                Text = " ",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
            };
            Lastest.Text = Last[0].ToString() + "-" + Last[1].ToString()
            + "-" + Last[2].ToString() + " " + Last[3] + " "
            + Last[4].ToString() + " " + Last[5].ToString()
            + " " + Last[6].ToString();

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Main Page" }, Lastest, minichart
                }
            };
        }
    }
}