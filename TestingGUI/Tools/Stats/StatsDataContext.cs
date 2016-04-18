using TestingGUI.Tools.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stats;
using Stats.Interfaces;
using Stats.Models;

namespace TestingGUI.Tools.Stats
{
    public class StatsDataContext
    {
        private IStat<uint> _stat = new IntStat(1);
        private IStatModifier<uint> _modifier = new StatModifier<uint>() { Id = new Guid(), Apply = (x) => x + 1, Undo = (x) => x - 1 };
    }
}
