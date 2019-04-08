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

            /*public struct BPMRecords
            {
                int day, month, year, beat;
                double sBP, mBP;
                public BPMRecords(int x, int y, int z, double s, double m, int a)
                {
                    day = y;
                    month = x;
                    year = z;
                    sBP = s;
                    mBP = m;
                    beat = a;
                }

                public string ToStringArray()
                 {
                     return day.ToString();
                 }
                public string ToStringArray()
                {
                    string[] temp = new string[] {
                day.ToString(), month.ToString(), year.ToString(), sBP.ToString(), mBP.ToString(), beat.ToString() };
                    string result = string.Join(", ", temp);
                    return result;


                }
                public static object[] needString(BPMRecords recode)
                {
                    return recode.ToStringArray();
                }

            }

            public void records(BPMRecords[] recode)
            {
                recode[0] = new BPMRecords(3, 1, 2018, 138.02, 92.02, 105);

            }*/



            public GraphsPage()
        {
            //BPMRecords[] recode = new BPMRecords[380];
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

            var editor = new Label { Text = "loading..."};

            #region How to load a text file embedded resource
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GraphsPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "tempdata.txt");

            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            #endregion

            editor.Text = text;


            StackLayout stackLayout = new StackLayout {
                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold}*/
                         editor

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