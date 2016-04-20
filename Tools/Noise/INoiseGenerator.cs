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
        float[,] Generate(ulong seed);

        float GetValueAtPoint(ICoordinates coords);

        float GetValueAtPoint(uint x, uint y);
    }
}
