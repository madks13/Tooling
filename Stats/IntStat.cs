using Stats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats
{
    public class IntStat : Stat<uint>
    {
        public IntStat(uint baseValue = 0)
        {
            Base = baseValue;
        }
    }
}
