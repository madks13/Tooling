using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Noise.Models
{
    public class Grad
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double Dot2(double x, double y)
        {
            return X * x + Y * y;
        }

        public double Dot3(double x, double y, double z)
        {
            return X * x + Y * y + Z * z;
        }
    }
}
