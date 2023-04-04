using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model
{
    interface IGraphable
    {
        string getGraphString(int height, int width);
        double getCurrentValue();
        double getMaxValue();
    }
}
