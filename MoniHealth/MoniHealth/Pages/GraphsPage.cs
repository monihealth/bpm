using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microcharts;
using Microcharts.Forms;
using Entry = Microcharts.Entry;
using System.Text;

using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MoniHealth.Pages
{
    
    public class GraphsPage : ContentPage
    {
            public GraphsPage()
            {
                List <BPMRecords> record = new List <BPMRecords>();
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

                var editor = new Label { Text = "loading...", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
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
                    alltext = alltext+ "\n" + text;
                    record.Add(new BPMRecords(text));
                    count = count + 1;
                 }
                //text = reader.ReadLine();

                }
                #endregion
                editor.Text = alltext;

                //record.Add(new BPMRecords(text));
                //record.Add(new BPMRecords("1-Mar-18", 138.12, 85.12, 105));
                var Lastest = new Label
                {
                    Text = " ",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                };
                Lastest.Text = record[count-1].Date +" "+ record[count - 1].Time+ " "
                + record[count - 1].Systolic.ToString()+" "+ record[count - 1].Diastolic.ToString()
                +" "+ record[count - 1].HeartBeat.ToString();



                /*Button createButton = new Button
                {
                    Text = "Create Account",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    BorderWidth = 1,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                createButton.Clicked += OnButtonClicked;*/





                /*StackLayout stackLayout = new StackLayout { };

                stackLayout.Children.Add(editor);
                Content = new ScrollView
                {
                Content = stackLayout
                };*/

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

                };*/

                List<Entry> entries = new List<Entry> { };
                double findmin = 300;
                for (int i = 0; i <= 7; i++)
                {
                    entries.Add(new Entry((float)record[i].Systolic));
                    entries[i].Label = record[i].Date;
                    entries[i].ValueLabel = record[i].Systolic.ToString();
                    entries[i].Color = SKColor.Parse("#FF1493");

                    
                    if (findmin >= record[i].Systolic)
                    {
                        findmin = record[i].Systolic;
                    }
                }



                ChartView chart1 = new ChartView
            {
                    Chart = new LineChart { Entries = entries, MinValue = (int)findmin },
                    HeightRequest = 160,
                };
                
                /*try
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


                StackLayout stackLayout = new StackLayout
                {
                    Margin = new Thickness(20),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children =
                    {
                    
                        /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold}*/
                         chart1, /*Lastest, editor */
                    }
                };

                Content = new ScrollView
                {
                    Content = stackLayout,
                };


            }



    }


}