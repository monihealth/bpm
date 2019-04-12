using System;
using System.Collections.Generic;
using System.Text;

namespace MoniHealth
{
    class BPMRecords
    {

        private string date;
        private string time;
        private double sBP;
        private double dBP;
        private int beat;


        public BPMRecords(string temp)
        {
            string[] tempList = temp.Split(' ');

            Date = tempList[0];
            time = tempList[1];
            sBP = Convert.ToDouble(tempList[2]);
            dBP = Convert.ToDouble(tempList[3]);
            beat = Int32.Parse(tempList[4]);
        }

        public BPMRecords(string date, string time, double sBP, double dBP, int beat)
        {
            this.date = date;
            this.sBP = sBP;
            this.dBP = dBP;
            this.beat = beat;
        }
        
        public string Date
        {
            get { return date; }
            set { date = value; }
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
        
    }
}
