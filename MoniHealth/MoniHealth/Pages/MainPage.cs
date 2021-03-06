﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microcharts.Forms;
using MoniHealth.Models;
using Xamarin.Forms;
using Microcharts.Forms;

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

            string bpmStatus = (bloodpressurestatus( Double.Parse(Last[4].ToString()) , Double.Parse(Last[5].ToString())) );

            var Lastest = new Label
            {
                Text = "Your most recent blood pressure measurement taken on 3-5-2019 was 124/85 mmHG, with a heartrate of 99",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
            };
            /*Lastest.Text = Last[0].ToString() + "-" + Last[1].ToString()
            + "-" + Last[2].ToString() + " " + Last[3] + " "
            + Last[4].ToString() + " " + Last[5].ToString()
            + " " + Last[6].ToString();*/

            Content = new StackLayout
            {
                Margin = new Thickness(20),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    new Label { Text = "Welcome back to MoniHleath" },
                    new Label {Text ="" },
                    //new Label { Text = ("Your most recent blood pressure result was " + Last[4] + " / " + Last[5]) },
                    Lastest,
                    new Label { Text = ("Your blood pressure is " + bpmStatus+"\n")},
                    new Label { Text = "Previous 10 recorded Systolic Blood Pressure Levels"},
                    minichart
                }
            };

            string bloodpressurestatus(double Sys, double Dys)
            {
                if (Sys < 120 && Dys < 80)
                {
                    return "Normal, maintain your current lifestyle champ";
                }
                else if (Sys > 120 && Sys < 129 && Dys < 80)
                {
                    return "Elevated";
                }
                else if ((Sys > 130 && Sys < 139) || (Dys > 80 && Dys < 89))
                {
                    return "is in Stage 1 of high blood pressure, Hypertension";
                }
                else if (Sys > 140 || Dys > 90)
                {
                    return "is in Stage 2 of high blood pressure, Hypertension";
                }
                else
                    return "in Hypertensive Crisis, see a doctor as soon as possible";
            }

        }
    }
}