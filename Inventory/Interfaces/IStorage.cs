using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Tools.Observable;

namespace Inventory.Interfaces
{
    public interface IStorage : IEnumerable<IStorageSlot>, IObservableProperties, INotifyCollectionChanged
    {
        UInt64 MaxSize { get; }

        UInt64 FreeSlots { get; }

        int IndexOf(IStorageSlot item);

        UInt64 Add(IStorable item, UInt64 amount = 1, Boolean cancelOnOver = false);

        UInt64 Add(IStorageSlot item, Boolean cancelOnOver = false);

        Boolean Remove(int index);
        
        UInt64 Remove(IStorable item, UInt64 amount = 1, Boolean cancelOnUnder = false);

        UInt64 Remove(IStorageSlot item, Boolean cancelOnUnder = false);
        
        Boolean Switch(IStorageSlot first, IStorageSlot second);
    }
}
