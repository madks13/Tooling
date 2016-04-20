using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Metrics
{
    public class Size : ISize
    {
        private ulong _x;

        public ulong X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        private ulong _y;

        public ulong Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }
    }
}
