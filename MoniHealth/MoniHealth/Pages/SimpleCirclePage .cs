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

namespace MoniHealth.Pages
{
    public class SimpleCirclePage : ContentPage
    {

        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }

        public SimpleCirclePage()
        {
            Title = "Simple Circle";

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
            { Text = "loading...", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
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


            /*List<Entry> entries = new List<Entry> {
                new Entry((float)record[count-2].Systolic)
                {
                    Color = SKColor.Parse("#FF1493"),
                    Label = record[count-2].Date,
                    ValueLabel = record[count - 2].Systolic.ToString()
                },

                new Entry((float)record[count-1].Systolic)
                {
                    Color = SKColor.Parse("#AA1493"),
                    Label = record[count-1].Date,
                    ValueLabel = record[count - 1].Systolic.ToString()
                }

            };

            ChartView chart1 = new ChartView
            {
                Chart = new LineChart { Entries = entries }
            };
            try
            {
                Content = chart1;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }*/





            /*try
            {
                Content = new PlotView
                {
                    Model = new PlotModel { Title = "Hello, Forms!" },
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }*/
            /*
            LineSeries line1 = new LineSeries()
            {
                Color = OxyColor.FromArgb(168, 9, 27, 1),
                DataFieldX = "Date",
                DataFieldY = "Systolic"
            };
            
            for (int i = 0; i <= 7; i++)
            {
                line1.Points.Add(new DataPoint(i, record[i].Systolic));
            }
            LinearAxis linearAxisY = new LinearAxis
            {
                Title = "Systolic",
                Position = AxisPosition.Left,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };
            LinearAxis linearAxisX = new LinearAxis
            {
                Title = "Date",
                Position = AxisPosition.Bottom,

            };

            StackLayout stackLayout = new StackLayout
            {
                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                    {
                    
                        /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold}
                          /*Lastest, editor
                    }
            };


            PlotModel mode = new PlotModel() { Title = "Systolic" };
            mode.Series.Add(line1);
            mode.Axes.Add(linearAxisY);
            mode.Axes.Add(linearAxisX);
            try
            {
                Content = new PlotView
                {
                    Model = mode,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }

            /*Content = new ScrollView
            {
                Content = stackLayout,
            };*/
            

                var start = new DateTime(2010, 01, 01);
                // Create a random data collection
                var data = new Collection< DateValue >();
                var date = start;
                for(int i = 0; i<=8;i++)
                {
                data.Add(new DateValue { Date = (new DateTime(record[i].Year, record[i].Month, record[i].Day)), Value = record[i].Systolic });
                }

                var plotModel1 = new PlotModel() { Title = "Systolic" };
            LinearAxis linearAxisY = new LinearAxis
            {
                Title = "Systolic",
                Position = AxisPosition.Left,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };

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
                plotModel1.Series.Add(lineSeries1);


            try
            {
                Content = new PlotView
                {
                    Model = plotModel1,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
            }





        }



    }
}