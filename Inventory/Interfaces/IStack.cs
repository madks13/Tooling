using System;
using Tools.Observable;

namespace Inventory.Interfaces
{
    public interface IStack : IObservableProperties
    {
        /// <summary>
        /// Returns the current amount in the stack
        /// </summary>
        UInt64 Current { get; set; }

        /// <summary>
        /// Returns the maximum amount that can be stacked
        /// </summary>
        UInt64 Left { get; }

        /// <summary>
        /// Returns the amount that can be added before reaching the maximum amount stackable
        /// </summary>
        UInt64 Max { get; set; }

        /// <summary>
        /// Adds amount to the stack
        /// </summary>
        /// <param name="amount">The amount to add to the stack</param>
        /// <param name="cancelOnOver">If false will add as much as possible to the stack, otherwise will not add anything</param>
        /// <returns>Returns the amount added ot the stack</returns>
        UInt64 Add(UInt64 amount, Boolean cancelOnOver = false);

        /// <summary>
        /// Removes amount from the stack
        /// </summary>
        /// <param name="amount">The amount to remove from the stack</param>
        /// <param name="cancelOnUnder">If false will remove as much as possible from the stack, otherwise will not remove anything</param>
        /// <returns>Returns the amoun not removed</returns>
        UInt64 Remove(UInt64 amount, Boolean cancelOnUnder = false);
    }
}