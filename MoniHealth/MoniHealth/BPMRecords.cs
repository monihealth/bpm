using System;
using System.Collections.Generic;
using System.Text;

namespace MoniHealth
{
    class BPMRecords
    {
        private DateTime alldate;
        private string date;
        private string time;
        private double sBP;
        private double dBP;
        private int beat;
        private int year;
        private int month;
        private int day;


        public BPMRecords(string temp)
        {
            string[] tempList = temp.Split(' ');

            /*Date = tempList[0];
            time = tempList[1];
            sBP = Convert.ToDouble(tempList[2]);
            dBP = Convert.ToDouble(tempList[3]);
            beat = Int32.Parse(tempList[4]);*/

            month = Int32.Parse(tempList[0]);
            day = Int32.Parse(tempList[1]);
            year = Int32.Parse(tempList[2]);
            alldate = new DateTime(year, month, day);
            time = tempList[3];
            sBP = Convert.ToDouble(tempList[4]);
            dBP = Convert.ToDouble(tempList[5]);
            beat = Int32.Parse(tempList[6]);

        }

        public BPMRecords(string date, string time, double sBP, double dBP, int beat)
        {
            this.date = date;
            this.sBP = sBP;
            this.dBP = dBP;
            this.beat = beat;
        }

        public BPMRecords(int year, int month, int day, string time, double sBP, double dBP, int beat)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.alldate = new DateTime(year, month, day);
            this.sBP = sBP;
            this.dBP = dBP;
            this.beat = beat;
        }

        public BPMRecords(){}

        /*public string Date
        {
            get { return date; }
            set { date = value; }
        }*/

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        public int Day
        {
            get { return day; }
            set { day = value; }
        }

        public DateTime AllDate
        {
            get { return alldate; }
            set { alldate = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public double Systolic
        {
            get { return sBP; }
            set { sBP = value; }
        }

        public double Diastolic
        {
            get { return dBP; }
            set { dBP = value; }
        }

        public int HeartBeat
        {
            get { return beat; }
            set { beat = value; }
        }

        public string readingToString()
        {
            string temp = Month.ToString() + "-" + Day.ToString()
            + "-" + Year.ToString() + " " + Time + " "
            + Systolic.ToString() + " " + Diastolic.ToString()
            + " " + HeartBeat.ToString();
            return temp;
        }
        
    }
}
