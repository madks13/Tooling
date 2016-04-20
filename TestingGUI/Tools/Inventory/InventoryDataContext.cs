using Inventory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Controls;
using Inventory.Interfaces;
using Inventory.Models;
using Tools.Observable;

namespace TestingGUI.Tools
{
    //public class ObservableInventory : ObservableProperties, IStorage, INotifyCollectionChanged
    //{
    //    #region Fields

    //    private IStorage _storage;

    //    #endregion

    //    #region C/Dtor

    //    public ObservableInventory(IStorage storage)
    //    {
    //        _storage = storage;
    //        _storage.PropertyChanged += _storage_PropertyChanged;
    //    }

    //    private void _storage_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        OnPropertyChanged(e.PropertyName);
    //    }

    //    public ulong FreeSlots
    //    {
    //        get
    //        {
    //            return _storage.FreeSlots;
    //        }
    //        set
    //        {
    //            _storage.FreeSlots = value;
    //        }
    //    }

    //    public ulong MaxSize
    //    {
    //        get
    //        {
    //            return _storage.MaxSize;
    //        }
    //    }

    //    #endregion

    //    #region INotifyCollectionChanged

    //    public event NotifyCollectionChangedEventHandler CollectionChanged;
        
    //    private void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
    //    {
    //        if (CollectionChanged != null)
    //        {
    //            CollectionChanged(this, a);
    //        }
    //    }

    //    #endregion

    //    #region IStorage

    //    public ulong Add(IStorageSlot item, bool cancelOnOver = false)
    //    {
    //        var previous = item.Stack.Current;
    //        var res = _storage.Add(item, cancelOnOver);
    //        if (res != previous)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public ulong Add(IStorable item, ulong amount = 1, bool cancelOnOver = false)
    //    {
    //        var res = _storage.Add(item, amount, cancelOnOver);
    //        if (res != amount)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public ulong Remove(IStorageSlot item, bool cancelOnUnder = false)
    //    {
    //        var previous = item.Stack.Current;
    //        var res = _storage.Remove(item, cancelOnUnder);
    //        if (res != previous)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public ulong Remove(IStorable item, ulong amount = 1, bool cancelOnUnder = false)
    //    {
    //        var res = _storage.Remove(item, amount, cancelOnUnder);
    //        if (res != amount)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public bool Switch(IStorageSlot first, IStorageSlot second)
    //    {
    //        var res = _storage.Switch(first, second);
    //        if (res)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public IEnumerator<IStorageSlot> GetEnumerator()
    //    {
    //        return _storage.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return _storage.GetEnumerator();
    //    }

    //    public bool Remove(int index)
    //    {
    //        var res = _storage.Remove(index);
    //        if (res)
    //        {
    //            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //        }
    //        return res;
    //    }

    //    public int IndexOf(IStorageSlot item)
    //    {
    //        return _storage.IndexOf(item);
    //    }

    //    #endregion
    //}

    public class InventoryDataContext
    {
        public InventoryDataContext(UInt64 size)
        {
            Items = new ListStorage() {new HealthPotion()};
            Items2 = new ObservableStorage(new TableStorage(5) { new ManaPotion()});
        }

        public UInt64 Quantity { get; set; } = 1;

        public ObservableCollection<IDisplayable> Choices { get; set; } = new ObservableCollection<IDisplayable>() { new HealthPotion(), new ManaPotion(), new Sword() };

        public IStorage Items { get; set; }

        public IStorage Items2 { get; set; }
    }
}
