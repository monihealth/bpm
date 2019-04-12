using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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


                StackLayout stackLayout = new StackLayout {
                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    
                    /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold}*/
                         Lastest, editor
                    }
                };

                Content = new ScrollView
                {
                    Content = stackLayout,
                };

                /*StackLayout stackLayout = new StackLayout { };

                stackLayout.Children.Add(editor);
                Content = new ScrollView
                {
                    Content = stackLayout
                };*/

            }



    }


}