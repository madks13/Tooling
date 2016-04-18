using System;
using Tools.Observable;

namespace Inventory.Interfaces
{
    public interface IStorable : IObservableProperties
    {
        /// <summary>
        /// Returns true if item is the same type as this object
        /// </summary>
        /// <param name="item">The item to compare with the current instance</param>
        /// <returns>True if the types are the same, false otherwise</returns>
        Boolean IsSameType(Object item);

        /// <summary>
        /// A simple check returning true if the object can be stacked with other objects of same type, or false if not
        /// </summary>
        Boolean CanStack { get; }

        /// <summary>
        /// Returns the maximum amount of objects of the object's type that can be stacked together
        /// </summary>
        UInt64 Stack { get; set; }
    }
}