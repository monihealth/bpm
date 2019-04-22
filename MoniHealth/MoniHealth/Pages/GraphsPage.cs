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
        private BPMRecords Last = new BPMRecords();
        Label typeOfGraphs = new Label() { FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        List<BPMRecords> Allrecord = new List<BPMRecords>();
        List<BPMRecords> record = new List<BPMRecords>();
        Label specificDates = new Label { Text = "", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        public DateTime start;
        public DateTime end;
        public int Graphs=0;
        Label avgOfSD = new Label() { Text = "", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        Label avgOfLastTen = new Label() { Text= "", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
        int count = 0;

        Button Input;
        DateTime inputDate;
        Label inputDateString;
        Label inputTimeString;
        TimePicker inputTimePicker;
        DatePicker inputDatePicker;
        Entry inputSystolic;
        Entry inputDiastolic;
        Entry inputHeartBeat;
        Button SubmitInput;
        StackLayout inputStack = new StackLayout()
        { VerticalOptions = LayoutOptions.StartAndExpand,
            Orientation = StackOrientation.Vertical,
        };

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
            
            #region Buttons and picker elements
            Button Submit = new Button
            {
                Text = "  Submit  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };

            var Start = new Label { Text = "Start Date:", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            DatePicker StartDate = new DatePicker
            {
                MinimumDate =Allrecord[0].AllDate,
                MaximumDate = Allrecord[count-1].AllDate,
                FontSize = 15,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Date = Allrecord[0].AllDate,
                

            };
            start = StartDate.Date;
            StartDate.DateSelected += StartDateChanged;

            var End = new Label { Text = "End Date:  ", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            DatePicker EndDate = new DatePicker
            {
                MinimumDate = StartDate.Date,
                MaximumDate = DateTime.Today,
                FontSize = 15,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Date = Allrecord[count - 1].AllDate
            };
            end = EndDate.Date;
            EndDate.DateSelected += EndDateChanged;

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
            #endregion

            #region Adding data
            Input = new Button
            {
                Text = "  Input  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            Input.Clicked += InputButton;
            inputStack.Children.Add(Input);
            inputDateString = new Label { Text = "Input Date:", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            inputDatePicker = new DatePicker
            {
                MinimumDate = new DateTime(2018, 1, 1),
                MaximumDate = DateTime.Today,
                FontSize = 15,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Date = DateTime.Today,


            };
            inputDate = DateTime.Today;
            inputDatePicker.DateSelected += InputDateChanged;
            inputTimeString = new Label { Text = "Input Time:", FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), VerticalOptions = LayoutOptions.Center };
            inputTimePicker = new TimePicker
            {
                FontSize = 15,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,

            };
            //inputTimePicker.SetBinding(TimePicker.TimeProperty, "StartTime");
            inputTimePicker.PropertyChanged += InputTimeChanged;

            inputSystolic = new Entry
            {
                Keyboard = Keyboard.Numeric,
                FontSize = 13,
                Placeholder = "Enter Systolic",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            inputDiastolic = new Entry
            {
                Keyboard = Keyboard.Numeric,
                FontSize = 13,
                Placeholder = "Enter Diastolic",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            inputHeartBeat = new Entry
            {
                Keyboard = Keyboard.Numeric,
                FontSize = 13,
                Placeholder = "Enter HeartBeat",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            SubmitInput = new Button
            {
                Text = "  Submit Input  ",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                BorderWidth = 1,
                BorderColor = Color.Silver,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            SubmitInput.Clicked += SubInputButton;
            #endregion


            avgOfLastTen.Text = AverageLastTen();

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
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    /*new Label { Text = (recode[0].ToStringArray()),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold}*/
                    /*chart1,*/ Lastest, avgOfLastTen,
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={Start, StartDate}},
                    new StackLayout(){ HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal, Children={End, EndDate} },
                    typeOfGraphs, typeOfGraph, Submit, ViewGraph, avgOfSD, specificDates,
                    inputStack
                }
            };

            Content = new ScrollView
            {
                Content = stackLayout,
                Margin = new Thickness(0, 0, 0, 10)

            };


        }

        public string AverageLastTen()
        {
            var mostRec = Allrecord[count - 1].AllDate;
            var tenAgo = new DateTime(mostRec.Year, mostRec.Month, (mostRec.Day-10));
            List<BPMRecords> rerecord = new List<BPMRecords>();
            rerecord = Allrecord.Where(x => x.AllDate >= tenAgo && x.AllDate <= mostRec).ToList();
            int counter = 0;
            int avgSBP = 0;
            int avgDBP = 0;
            foreach (var reading in rerecord)
            {
                avgSBP = avgSBP + (int)reading.Systolic;
                avgDBP = avgDBP + (int)reading.Diastolic;
                counter++;
            }
            avgSBP = avgSBP / counter;
            avgDBP = avgDBP / counter;
            return ("Average Blood Pressure of last 10 \n" +  "readings: " + avgSBP.ToString() + "/" + avgDBP.ToString() + " mmHg");
        }


        public object[] LastRecord()
        {
            return new Object[] { Last.Year, Last.Month, Last.Day, Last.Time, Last.Systolic, Last.Diastolic, Last.HeartBeat };
        }

        void SubmitButton(object sender, EventArgs e)
        {
            specificDates.Text = "Date           Time mmHg        HB\n";
            record = Allrecord.Where(x => x.AllDate >= start && x.AllDate <= end).ToList();
            int counter = 0;
            int avgSBP = 0;
            int avgDBP = 0;
            foreach (var reading in record)
            {
                specificDates.Text = specificDates.Text + reading.readingToString() + "\n";
                avgSBP = avgSBP + (int)reading.Systolic;
                avgDBP = avgDBP + (int)reading.Diastolic;
                counter++;
            }
            if (counter>0)
            {
                avgSBP = avgSBP / counter;
                avgDBP = avgDBP / counter;
            }
            
            avgOfSD.Text = "Average Blood Pressure between \n" + start.ToShortDateString()
                + " and " + end.ToShortDateString() + ": \n" + avgSBP.ToString()+"/"+ avgDBP.ToString()+" mmHg";
        }
        void StartDateChanged(object sender, EventArgs e)
        {
            var picker = (DatePicker)sender;
            start = picker.Date;
        }

        void EndDateChanged(object sender, EventArgs e)
        {
            var picker = (DatePicker)sender;
            if (picker.Date< start)
            {
                picker.Date = start;
            }
            end = picker.Date;
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
                Graphs = picker.SelectedIndex;
            }
        }





        void InputButton(object sender, EventArgs e)
        {
            inputStack.Children.RemoveAt(0);
            inputStack.Children.Add(inputDateString);
            inputStack.Children.Add(inputDatePicker);
            inputStack.Children.Add(inputTimeString);
            inputStack.Children.Add(inputTimePicker);
            inputStack.Children.Add(inputSystolic);
            inputStack.Children.Add(inputDiastolic);
            inputStack.Children.Add(inputHeartBeat);
            inputStack.Children.Add(SubmitInput);
        }

        void InputDateChanged(object sender, EventArgs e)
        {
            var picker = (DatePicker)sender;
            inputDate = picker.Date;
        }
        void InputTimeChanged(object sender, EventArgs e)
        {
            var picker = (TimePicker)sender;
            inputDate = inputDate.Add(picker.Time);
        }
        void SubInputButton(object sender, EventArgs e)
        {
            inputStack.Children.Clear();
            var newda = new Label() {Text = inputDate.ToShortDateString()+ inputDate.ToShortTimeString() };
            inputStack.Children.Add(Input);
            inputStack.Children.Add(newda);

        }




    }


}

