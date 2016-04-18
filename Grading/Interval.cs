using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grading
{
    public class Interval
    {
        #region Fields

        private Double _min = 0;
        private Double _max = 0;

        #endregion

        #region Properties

        public Double Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (value > _max)
                {
                    _min = _max;
                    _max = value;
                }
                else
                {
                    _min = value;
                }
            }
        }

        public Double Max
        {
            get
            {
                return _max;
            }
            set
            {
                if (value < _min)
                {
                    _max = _min;
                    _min = value;
                }
                else
                {
                    _max = value;
                }
            }
        }

        #endregion

        #region Methods

        public Interval(Double min, Double max)
        {
            Min = min;
            Max = max;
        }

        public static Double GetValue(Double rawValue, Double minValue, Double maxValue)
        {
            if (rawValue <= minValue)
            {
                return minValue;
            }
            if (rawValue >= maxValue)
            {
                return maxValue;
            }
            return rawValue;
        }

        public Double GetValue(Double rawValue)
        {
            return GetValue(rawValue, _min, _max);
        }

        #endregion
    }
}
