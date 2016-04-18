using System;
using Tools.Observable;

namespace Inventory.Interfaces
{
    public interface IStorageSlot : IObservableProperties
    {
        /// <summary>
        /// Returns the object stored in the slot
        /// </summary>
        IStorable Item { get; set; }

        /// <summary>
        /// Returns the stack of the object
        /// </summary>
        IStack Stack { get; }

        /// <summary>
        /// Sets item as the item the StorageSlot is holding
        /// </summary>
        /// <param name="item">The item the StorageSlot will be holding</param>
        /// <param name="amount">The size of the item's current stack</param>
        void SetItem(IStorable item, UInt64 amount = 1);

        /// <summary>
        /// Adds amount to the stack of the object
        /// </summary>
        /// <param name="amount">The amount to be added to the stack</param>
        /// <param name="cancelOnOver">If false will add as much as possible to the stack, otherwise if amount is bigger than this stack's left space will cancel the changes</param>
        /// <returns>Returns the amount that was not added</returns>
        UInt64 Add(UInt64 amount, bool cancelOnOver = false);

        /// <summary>
        /// Removes amount from this object's stack
        /// </summary>
        /// <param name="amount">The amount to remove from this stack</param>
        /// <param name="cancelOnUnder">If false will remove as much as possible from the stack, otherwise if amount if bigger than current stack will cancel the changes</param>
        /// <returns>Returns the amount not removed from the stack</returns>
        UInt64 Remove(UInt64 amount, bool cancelOnUnder = false);

        event EventHandler ItemSwitched;

        event EventHandler ItemChanged;
    }
}