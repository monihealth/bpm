using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using Entry = Microcharts.Entry;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Microcharts;
using Microcharts.Forms;
using System;
using OxyPlot.Xamarin.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoniHealth.Models;

namespace MoniHealth.Pages
{
    public class SimpleCirclePage : ContentPage
    {

        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
            public double Value2 { get; set; }
            public double Value3 { get; set; }
        }

        public SimpleCirclePage()
        {
            Title = "Graph";
            //GraphsPage gif = new GraphsPage();

            //var starting = new Label() ;
            //var ending = new Label();
            //starting = TabPage.gif.startlable;
            //ending = TabPage.gif.endlable;
            SKCanvasView canvasView = new SKCanvasView();
            Content = canvasView;

            List<BPMRecords> record = new List<BPMRecords>();
            //BPMRecords recode = new BPMRecords(3, 1, 2018, 138.02, 92.02, 105);
            //records(recode);
            
            #if __IOS__
                var resourcePrefix = "MoniHealth.iOS.Resources.";
            #endif

            #if __ANDROID__
                var resourcePrefix = "MoniHealth.Android.Resources.";
            #else
                var resourcePrefix = "MoniHealth.Pages.";
            #endif

            var editor = new Label
            {
                Text = "loading...",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
            };

            #region How to load a text file embedded resource
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GraphsPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "tempdata.txt");

            string text = "";
            string alltext = "";
            int count = 0;
            using (var reader = new StreamReader(stream))
            {
                while ((text = reader.ReadLine()) != null)
                {
                    alltext = alltext + "\n" + text;
                    record.Add(new BPMRecords(text));
                    count = count + 1;
                }
            }
            #endregion

            record = record.Where(x => x.AllDate >= TabPage.gif.start && x.AllDate <= TabPage.gif.end).ToList();


            Button backButton = new Button
            {
                Text = "  Back  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(15, 0, 0, 0)
            };
            backButton.Clicked += OnDismissClicked;


            //var start = new DateTime(2010, 01, 01);
            // Create a random data collection
            var data = new Collection<DateValue>();
            var dataType2 = new Collection<DateValue>();
            var dataType3 = new Collection<DateValue>();
            string titleString="";
            //var date = start;
            /*foreach(var reading in record)
            {
                data.Add(new DateValue { Date = reading.AllDate, Value = reading.Systolic });
            }*/
            switch (TabPage.gif.Graphs)
            {
                case 0:
                    foreach(var reading in record)
                    {
                        data.Add(new DateValue { Date = reading.AllDate, Value = reading.Systolic });
                    }
                    titleString = "Systolic";
                    break;
                case 1:
                    foreach (var reading in record)
                    {
                        data.Add(new DateValue { Date = reading.AllDate, Value = reading.Diastolic });
                    }
                    titleString = "Diastolic";
                    break;
                case 2:
                    foreach (var reading in record)
                    {
                        data.Add(new DateValue { Date = reading.AllDate, Value = reading.HeartBeat });
                    }
                    titleString = "HeartBeat";
                    break;

                case 3:
                    foreach (var reading in record)
                    {
                        data.Add(new DateValue { Date = reading.AllDate, Value = reading.Systolic });
                        dataType2.Add(new DateValue { Date = reading.AllDate, Value = reading.Diastolic });
                        dataType3.Add(new DateValue { Date = reading.AllDate, Value = reading.HeartBeat });
                    }
                    titleString = "All";
                    break;
            }


            var plotModel1 = new PlotModel() { Title = titleString };
            LinearAxis linearAxisY = new LinearAxis
            {
                Title = titleString,
                Position = AxisPosition.Left,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };
            linearAxisY.IsZoomEnabled = false;
            linearAxisY.IsPanEnabled = false;
            var dateTimeAxis1 = new DateTimeAxis
            {
                Title = "Date",
                CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
                FirstDayOfWeek = DayOfWeek.Monday,
                Position = AxisPosition.Bottom
            };
            plotModel1.Axes.Add(dateTimeAxis1);
            plotModel1.Axes.Add(linearAxisY);
            var lineSeries1 = new LineSeries
            {
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus,
                StrokeThickness = 1,
                DataFieldX = "Date",
                DataFieldY = "Value",
                ItemsSource = data
            };

            if (TabPage.gif.Graphs ==3)
            {
                var lineSeries2 = new LineSeries
                {
                    MarkerStroke = OxyColors.Coral,
                    MarkerType = MarkerType.Star,
                    StrokeThickness = 1,
                    DataFieldX = "Date",
                    DataFieldY = "Value",
                    ItemsSource = dataType2
                };
                var lineSeries3 = new LineSeries
                {
                    MarkerStroke = OxyColors.AliceBlue,
                    MarkerType = MarkerType.Diamond,
                    StrokeThickness = 1,
                    DataFieldX = "Date",
                    DataFieldY = "Value",
                    ItemsSource = dataType3
                };
                plotModel1.Series.Add(lineSeries2);
                plotModel1.Series.Add(lineSeries3);
            }
            plotModel1.Series.Add(lineSeries1);

            var grid = new Grid() { Margin = new Thickness(20), VerticalOptions = LayoutOptions.FillAndExpand };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            try
            {
                grid.Children.Add(new PlotView
                {
                    Model = plotModel1,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                }, 0, 0);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }
            grid.Children.Add(backButton, 0, 1);

            Content = grid;
            /*try
            {
                //Content = loginButton;
                Content = new PlotView
                {
                    Model = plotModel1,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                };
                Content = loginButton;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }*/

        }

        async void OnDismissClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }



    }
}

