using System;

namespace Stats.Interfaces
{
    public interface IStat<T>
    {
        T Base { get; set; }

        T Current { get; set; }

        string Name { get; set; }

        Boolean AddModifier(IStatModifier<T> modifier);
    }
}