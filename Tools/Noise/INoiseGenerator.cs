using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Metrics;

namespace Tools.Noise
{
    public interface INoiseGenerator
    {
        void Seed(double seed);

        double GetValueAtPoint(ICoordinates coords);

        double GetValueAtPoint(double x, double y);
    }
}
