using Stats.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Models
{
    public class StatModifier<T> : IStatModifier<T>
    {
        public Guid Id { get; set; }

        public Func<T, T> Apply { get; set; }

        public Func<T, T> Undo { get; set; }
    }
}
