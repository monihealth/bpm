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

namespace MoniHealth.Pages
{
    public class SimpleCirclePage : ContentPage
    {
        public SimpleCirclePage()
        {
            /*Title = "Simple Circle";

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


            List<Entry> entries = new List<Entry> {
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

        }

        
        
    }
}