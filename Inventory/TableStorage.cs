﻿using Inventory.Interfaces;
using Inventory.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Observable;
using System.Collections.Specialized;

namespace Inventory
{
    public class TableStorage : ObservableProperties, IStorage
    {
        #region Fields

        private IStorageSlot[] _storage;
        private ulong _freeSlots;

        #endregion

        #region C/Dtor

        public TableStorage(UInt64 size)
        {
            _storage = new IStorageSlot[size];
            for (int i = 0; i < _storage.Length; i++)
            {
                _storage[i] = new StorageSlot(null);
                _storage[i].ItemChanged += TableStorage_ItemChanged; ;
            }
            FreeSlots = size;
        }
        
        #endregion

        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
        {
            CollectionChanged?.Invoke(this, a);
        }

        #endregion

        #region Event listeners

        private void TableStorage_ItemChanged(object sender, EventArgs e)
        {
            IStorageSlot slot = sender as IStorageSlot;
            if (slot.Item == null)
            {
                FreeSlots += 1;
            }
            else
            {
                FreeSlots -= 1;
            }
        }

        #endregion

        #region IStorage 

        #region Properties

        public UInt64 MaxSize
        {
            get
            {
                return (UInt64)_storage.Length;
            }
        }

        public UInt64 FreeSlots
        {
            get
            {
                return _freeSlots;
            }
            set
            {
                SetField(ref _freeSlots, value);
            }
        }

        #endregion

        #region Methods

        #region Private

        private IEnumerable<IStorageSlot> FindEmptySlots()
        {
            return _storage.Where(s => s.Item == null);
        }

        private IStorageSlot GetEmptySlot()
        {
            return FindEmptySlots().FirstOrDefault();
        }

        private Boolean AddInEmptySlot(IStorable item, UInt64 amount)
        {
            if (FreeSlots > 0)
            {
                var emptySlot = GetEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.SetItem(item, amount);
                    return true;
                }
            }
            return false;
        }

        private Boolean AddInEmptySlot(IStorageSlot item)
        {
            return AddInEmptySlot(item.Item, item.Stack.Current);
        }

        private UInt64 AddStackable(IStorable item, UInt64 amount, Boolean cancelOnOver)
        {
            var left = amount;
            var list = _storage.Where(i => i.Item != null && i.Item.IsSameType(item));

            foreach (var element in list)
            {
                left = element.Add(left, cancelOnOver);
                if (left <= 0)
                {
                    break;
                }
            }

            if (left > 0)
            {
                amount = left;
                var res = AddInEmptySlot(item, amount);
                if (res)
                {
                    left = 0;
                }
            }
            return left;
        }

        private UInt64 AddStackable(IStorageSlot item, Boolean cancelOnOver)
        {
            return AddStackable(item.Item, item.Stack.Current, cancelOnOver);
        }

        private UInt64 RemoveStackable(IStorable item, UInt64 amount, Boolean cancelOnUnder)
        {
            var left = amount;
            var list = _storage.Where(i => i.Item != null && i.Item.IsSameType(item));

            foreach (var element in list)
            {
                left = element.Remove(left, cancelOnUnder);
                if (left <= 0)
                {
                    break;
                }
            }

            return left;
        }

        #endregion

        #region Public

        #region Add

        public UInt64 Add(IStorable item, UInt64 amount = 1, Boolean cancelOnOver = false)
        {
            if (item != null)
            {
                if (item.CanStack)
                {
                    amount = AddStackable(item, amount, cancelOnOver);
                }
                if (amount > 0)
                {
                    var res = AddInEmptySlot(item, amount);
                    if (res)
                    {
                        amount = 0;
                    }
                }
            }
            return amount;
        }

        public UInt64 Add(IStorageSlot item, Boolean cancelOnOver = false)
        {
            if (item != null
                && item.Item != null)
            {
                return Add(item.Item, item.Stack.Current, cancelOnOver);
            }
            return item.Stack.Current;
        }

        #endregion
        
        #region Remove

        public Boolean Remove(int index)
        {
            if (index >= 0 && index < _storage.Length)
            {
                _storage[index].SetItem(null);
                return true;
            }
            return false;
        }

        public UInt64 Remove(IStorable item, UInt64 amount = 1, Boolean cancelOnUnder = false)
        {
            if (item != null)
            {
                if (item.CanStack)
                {
                    amount = RemoveStackable(item, amount, cancelOnUnder);
                }
            }
            return amount;
        }

        public UInt64 Remove(IStorageSlot item, Boolean cancelOnUnder = false)
        {
            if (item != null
               && item.Item != null)
            {
                return Remove(item.Item, item.Stack.Current, cancelOnUnder);
            }
            return item.Stack.Current;
        }

        #endregion
        
        public int IndexOf(IStorageSlot item)
        {
            for (int i = 0; i < _storage.Length; i++)
            {
                if (Object.ReferenceEquals(item, _storage[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public Boolean Switch(IStorageSlot first, IStorageSlot second)
        {
            if (first != null
                && second != null)
            {
                if (!Object.ReferenceEquals(first, second))
                {
                    if (first.Item != null && second.Item != null && first.Item.IsSameType(second.Item))
                    {
                        second.Stack.Current = first.Add(second.Stack.Current);
                        return true;
                    }
                    else if (first.Item != null
                        || second.Item != null)
                    {
                        var tmpItem = first.Item;
                        var tmpStack = first.Stack.Current;
                        first.SetItem(second.Item, second.Stack.Current);
                        second.SetItem(tmpItem, tmpStack);
                        return true;
                    }
                }
            }
            return false;
        }
        
        #endregion

        #endregion

        #endregion

        #region IEnumerable<IStorageSlot> interface

        public IEnumerator<IStorageSlot> GetEnumerator()
        {
            foreach (IStorageSlot item in _storage)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion
    }
}
