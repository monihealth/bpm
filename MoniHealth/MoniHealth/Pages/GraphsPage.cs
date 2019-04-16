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
using System.Text;

using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;
using Entry = Xamarin.Forms.Entry;
using System.Collections.ObjectModel;

namespace MoniHealth.Pages
{

    public class GraphsPage : ContentPage
    {
        //GraphsPage gif = new GraphsPage();
        private BPMRecords Last = new BPMRecords();
        Label typeOfGraphs = new Label() { FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        List<BPMRecords> Allrecord = new List<BPMRecords>();
        List<BPMRecords> record = new List<BPMRecords>();
        Label specificDates = new Label { Text = "", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        public DateTime start;
        public DateTime end;
        public Label startlable = new Label() { Text = "" };
        public Label endlable = new Label() { Text = "" };

        public class StartAndEnding
        {
            public string Date1;
            public string Date2;
        }

        public GraphsPage()
        {
            Title = "BP Readings";

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

            var editor = new Label { Text = "loading...", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };

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
                    Allrecord.Add(new BPMRecords(text));
                    count = count + 1;
                }
                //text = reader.ReadLine();

            }
            #endregion
            editor.Text = alltext;

            //record.Add(new BPMRecords(text));
            //record.Add(new BPMRecords("1-Mar-18", 138.12, 85.12, 105));
            var Lastest = new Label { Text = " ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };


            Button Submit = new Button
            {
                Text = "Submit",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };


            /*Entry StartDate = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter Start Date",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Entry EndDate = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter End Date",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };*/
            var Start = new Label { Text = "Start Date:", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            DatePicker StartDate = new DatePicker
            {
                MinimumDate = new DateTime(2018, 1, 1),
                MaximumDate = new DateTime(2019, 12, 31),
                FontSize = 10,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,

                //Date = new DateTime(2018, 6, 21)

            };
            StartDate.DateSelected += StartDateChanged;

            var End = new Label { Text = "End Date:", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            DatePicker EndDate = new DatePicker
            {
                MinimumDate = new DateTime(2018, 1, 1),
                MaximumDate = new DateTime(2019, 12, 31),
                FontSize = 10,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
                //Date = new DateTime(2018, 6, 21)
            };
            EndDate.DateSelected += EndDateChanged;

            Button ViewGraph = new Button
            {
                Text = "ViewGraph",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            ViewGraph.Clicked += ViewGraphButton;

            Picker typeOfGraph = new Picker
            {
                Title = "Type of Data in Graph",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            typeOfGraph.Items.Add("Systolic");
            typeOfGraph.Items.Add("Diastolic");
            typeOfGraph.Items.Add("HeartBeat");
            typeOfGraph.Items.Add("All");
            typeOfGraph.SelectedIndex = 0;
            typeOfGraphs.Text = "Graph of " + typeOfGraph.Items[typeOfGraph.SelectedIndex];
            typeOfGraph.SelectedIndexChanged += TypeOfGraphChanged;

            Lastest.Text = Allrecord[count - 1].readingToString();

            Last = Allrecord[count - 1];
            //start = StartDate.Date;
            //end = EndDate.Date;
            Submit.Clicked += SubmitButton;

            ;

            /*record = Allrecord.Where(x => x.AllDate >= StartDate.Date && x.AllDate <= EndDate.Date).ToList();
            foreach (var reading in record)
            {
                specificDates.Text = specificDates.Text + reading.readingToString() + "\n";
            }*/

            #region Old greaphs method
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

            /*List<Entry> entries = new List<Entry> 
             * {new Entry((float)record[count-2].Systolic)
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

            /*List<Entry> entries = new List<Entry> { };
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
                Chart = new LineChart { Entries = entries, MinValue = (int)findmin,  },
                HeightRequest = 160,
                //Chart.DrawCaptionElements()

            };*/

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
            #endregion

            StackLayout stackLayout = new StackLayout
            {

                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold}*/
                    /*chart1,*/ Lastest,
                    new StackLayout(){ HorizontalOptions = LayoutOptions.Start,
                    Orientation = StackOrientation.Horizontal, Children={Start, StartDate}},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.Start,
                    Orientation = StackOrientation.Horizontal, Children={End, EndDate} },
                    typeOfGraphs, typeOfGraph, Submit, ViewGraph, specificDates, //editor 
                }
            };

                Content = new ScrollView
                {
                    Content = stackLayout,
                };


        }



        public object[] LastRecord()
        {
            return new Object[] { Last.Year, Last.Month, Last.Day, Last.Time, Last.Systolic, Last.Diastolic, Last.HeartBeat };
        }

        void SubmitButton(object sender, EventArgs e)
        {
            specificDates.Text = "";
            record = Allrecord.Where(x => x.AllDate >= start && x.AllDate <= end).ToList();
            foreach (var reading in record)
            {
                specificDates.Text = specificDates.Text + reading.readingToString() + "\n";
            }
        }
        void StartDateChanged(object sender, EventArgs e)
        {
            var picker = (DatePicker)sender;
            start = picker.Date;
            startlable.Text = start.ToShortDateString();
        }

        void EndDateChanged(object sender, EventArgs e)
        {
            var picker = (DatePicker)sender;
            end = picker.Date;
            endlable.Text = end.ToShortDateString();
        }

        async void ViewGraphButton(object sender, EventArgs e)
        {
            var simp = new SimpleCirclePage();
            await Navigation.PushAsync(simp);
        }

        void TypeOfGraphChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                typeOfGraphs.Text = "Graph of " + picker.Items[selectedIndex];
            }
        }
    }


}

/*
namespace MoniHealth.Pages
{

    public class GraphsPage : ContentPage
    {
        private BPMRecords Last = new BPMRecords();


        public GraphsPage()
        {
            Title = "BP Readings";
            List<BPMRecords> Allrecord = new List<BPMRecords>();
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

            var editor = new Label { Text = "loading...", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };

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
                    Allrecord.Add(new BPMRecords(text));
                    count = count + 1;
                }
                //text = reader.ReadLine();

            }
            #endregion
            editor.Text = alltext;

            //record.Add(new BPMRecords(text));
            //record.Add(new BPMRecords("1-Mar-18", 138.12, 85.12, 105));
            var Lastest = new Label { Text = " ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            var specificDates = new Label { Text = "", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };


            Button Submit = new Button
            {
                Text = "  Submit  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };

            Entry StartDate = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter Start Date",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Entry EndDate = new Entry
            {
                Keyboard = Keyboard.Text,
                FontSize = 10,
                Placeholder = "Enter End Date",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };

            Button ViewGraph = new Button
            {
                Text = "  ViewGraph  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            ViewGraph.Clicked += ViewGraphButton;



            Lastest.Text = Allrecord[count - 1].readingToString();

            Last = Allrecord[count - 1];

            record = Allrecord.Where(x => x.Month == 6 && x.Year == 2018).ToList();
            foreach (var reading in record)
            {
                specificDates.Text = specificDates.Text + reading.readingToString() + "\n";
            }*/


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

/*List<Entry> entries = new List<Entry> 
 * {new Entry((float)record[count-2].Systolic)
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

/*List<Entry> entries = new List<Entry> { };
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
    Chart = new LineChart { Entries = entries, MinValue = (int)findmin,  },
    HeightRequest = 160,
    //Chart.DrawCaptionElements()

};*/

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
}*//*


StackLayout stackLayout = new StackLayout
{

    Margin = new Thickness(20),
    VerticalOptions = LayoutOptions.StartAndExpand,
    Children =
    {*/
   /*new Label { Text = (recode[0].ToStringArray()),
   FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
   FontAttributes = FontAttributes.Bold}*/
   /*chart1,*//* Lastest, StartDate, EndDate, Submit, ViewGraph, specificDates, editor
}
};

Content = new ScrollView
{
Content = stackLayout,
};


}

public object[] LastRecord()
{
return new Object[] { Last.Year, Last.Month, Last.Day, Last.Time, Last.Systolic, Last.Diastolic, Last.HeartBeat };
}

async void ViewGraphButton(object sender, EventArgs e)
{
await Navigation.PushAsync(new SimpleCirclePage());
}


}


}*/
