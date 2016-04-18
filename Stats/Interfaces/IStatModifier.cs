using System;

namespace Stats.Interfaces
{
    public interface IStatModifier<T>
    {
        Func<T, T> Apply { get; set; }
        Guid Id { get; set; }
        Func<T, T> Undo { get; set; }
    }
}