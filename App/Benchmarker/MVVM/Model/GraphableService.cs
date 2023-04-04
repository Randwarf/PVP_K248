﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model
{
    internal class GraphableService : IGraphable
    {
        protected LinkedList<double> Values;
        protected int NextCalls = 0;
        public double average { get; private set; }
        

        public GraphableService(int listSize = 100)
        {
            Values = new LinkedList<double>( Enumerable.Repeat(0.0, listSize) );
            average = 0;
        }

        public void calculateNext()
        {
            double valueToAdd = getRawNext(); 
            Values.AddLast(valueToAdd);
            Values.RemoveFirst();
            UpdateAverage(valueToAdd);
        }

        protected void UpdateAverage(double newValue)
        {
            average = (average * NextCalls + newValue) / (NextCalls + 1);
            NextCalls++;
        }

        //Classes that inherit GraphableService class must overide gerRawNext method
        protected virtual double getRawNext()
        {
            return 1;
        }

        public string getGraphString(int height, int width)
        {
            double maxValue = Values.Max();
            if (maxValue <= 0) maxValue = 0.0001;
            double heightRatio = (double)height / maxValue;
            double widthRatio = (double)width / Values.Count;
            var builder = new StringBuilder();
            for (int i = 0; i < Values.Count(); i++)
            {
                double yPos = (maxValue - Values.ElementAt(i)) * heightRatio;
                string yPosWithDot = yPos.ToString().Replace(",", ".");

                double xPos = i * widthRatio;
                string xPosWithDot = xPos.ToString().Replace(",", ".");
                builder.Append(xPosWithDot + "," + yPosWithDot + " ");
            }
            return builder.ToString();
        }

        public double getCurrentValue()
        {
            return Values.Last();
        }

        public double getMaxValue()
        {
            return Values.Max();
        }
    }
}
