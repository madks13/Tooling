using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Tools.Observable;

namespace Inventory.Interfaces
{
    public interface IStorage : IEnumerable<IStorageSlot>, IObservableProperties, INotifyCollectionChanged
    {
        #region Metrics

        UInt64 MaxSize { get; }

        UInt64 FreeSlots { get; set; }

        #endregion

        #region Rules

        Func<IStorable, Boolean> Conditions { get; set; }

        Func<IStorable, Boolean> Priorities { get; set; }

        Boolean CheckPriorities(IStorable storable);

        Boolean CheckPriorities(IStorageSlot slot);

        Boolean CheckConditions(IStorable storable);

        Boolean CheckConditions(IStorageSlot slot);

        #endregion

        #region Search

        int IndexOf(IStorageSlot item);

        #endregion

        #region Manipulation

        UInt64 Add(IStorable item, UInt64 amount = 1, Boolean cancelOnOver = false);

        UInt64 Add(IStorageSlot item, Boolean cancelOnOver = false);

        Boolean Remove(int index);
        
        UInt64 Remove(IStorable item, UInt64 amount = 1, Boolean cancelOnUnder = false);

        UInt64 Remove(IStorageSlot item, Boolean cancelOnUnder = false);
        
        Boolean Switch(IStorageSlot first, IStorageSlot second);

        #endregion
    }
}
