using Inventory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Tools.Observable;
using Inventory.Models;
using System.Collections.Specialized;

namespace Inventory
{
    public class ListStorage : ObservableProperties, IStorage
    {
        #region Fields

        private List<IStorageSlot> _storage;

        #endregion

        #region C/Dtor

        public ListStorage()
        {
            _storage = new List<IStorageSlot>();
        }

        #endregion

        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
        {
            CollectionChanged?.Invoke(this, a);
        }

        #endregion

        #region Event Listeners

        private void Slot_ItemChanged(object sender, EventArgs e)
        {
            IStorageSlot slot = sender as IStorageSlot;
            if (slot.Stack.Current == 0)
            {
                Remove(slot);
            }
        }

        #endregion

        #region Properties

        public UInt64 FreeSlots
        {
            get
            {
                return 0;
            }
        }

        public UInt64 MaxSize
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region Private

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
                AddInEmptySlot(item, left);
                left = 0;
            }
            return left;
        }

        private UInt64 AddStackable(IStorageSlot item, Boolean cancelOnOver)
        {
            var res = AddStackable(item.Item, item.Stack.Current, cancelOnOver);
            if (res > 0 && !cancelOnOver)
            {
                AddInEmptySlot(item);
                return 0;
            }

            return res;
        }

        private void AddInEmptySlot(IStorable item, UInt64 amount)
        {
            IStorageSlot newSlot = new StorageSlot(item, amount);
            newSlot.ItemChanged += Slot_ItemChanged; ;
            _storage.Add(newSlot);
        }
        
        private void AddInEmptySlot(IStorageSlot item)
        {
            _storage.Add(item);
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

        private UInt64 RemoveStackable(IStorageSlot item, Boolean cancelOnUnder)
        {
            return RemoveStackable(item.Item, item.Stack.Current, cancelOnUnder);
        }

        #endregion

        #region Public

        #region Add

        public ulong Add(IStorageSlot item, bool cancelOnOver = false)
        {
            if (item != null)
            {
                var previous = item.Stack.Current;
                var res = Add(item.Item, item.Stack.Current, cancelOnOver);
                if (res != previous)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return res;
            }
            return 0;
        }

        public ulong Add(IStorable item, ulong amount = 1, bool cancelOnOver = false)
        {
            if (item != null)
            {
                var res = amount;
                if (item.CanStack)
                {
                    res = AddStackable(item, amount, cancelOnOver);
                }
                if (res > 0)
                {
                    AddInEmptySlot(item, amount);
                    res = 0;
                }
                if (res != amount)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return res;
            }
            return amount;
        }

        #endregion

        #region Remove

        public bool Remove(int index)
        {
            var res = _storage.Remove(_storage.ElementAt(index));
            if (res)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public ulong Remove(IStorageSlot item, bool cancelOnUnder = false)
        {
            if (item != null)
            {
                var previous = item.Stack.Current;
                var res = _storage.Remove(item);
                if (res)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    return 0;
                }
                return previous;

            }
            return 0;
        }

        public ulong Remove(IStorable item, ulong amount = 1, bool cancelOnUnder = false)
        {
            if (item != null)
            {
                var res = RemoveStackable(item, amount, cancelOnUnder);
                if (res == 0)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return res;
            }
            return amount;
        }

        #endregion

        public int IndexOf(IStorageSlot item)
        {
            return _storage.IndexOf(item);
        }
        
        public bool Switch(IStorageSlot first, IStorageSlot second)
        {
            if (first != null
                && second != null)
            {
                if (!Object.ReferenceEquals(first, second))
                {
                    if (first.Item != null && second.Item != null && first.Item.IsSameType(second.Item))
                    {
                        var res = first.Add(second.Stack.Current);
                        if (res != second.Stack.Current)
                        {
                            second.Stack.Current = res;
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                            return true;
                        }
                        return false;
                    }
                    else if (first.Item != null
                        || second.Item != null)
                    {
                        var tmpItem = first.Item;
                        var tmpStack = first.Stack.Current;
                        first.SetItem(second.Item, second.Stack.Current);
                        second.SetItem(tmpItem, tmpStack);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        return true;
                    }
                }
            }
            return false;
        }

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

        #endregion
    }
}
