using Inventory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Observable;

namespace Inventory.Models
{
    public class ObservableStorage : ObservableProperties, IStorage, INotifyCollectionChanged
    {
        #region Fields

        private IStorage _storage;

        #endregion

        #region C/Dtor

        public ObservableStorage(IStorage storage)
        {
            _storage = storage;
            _storage.PropertyChanged += _storage_PropertyChanged;
        }

        #endregion

        #region Event listeners

        private void _storage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region Properties

        public ulong FreeSlots
        {
            get
            {
                return _storage.FreeSlots;
            }
            set
            {
                _storage.FreeSlots = value;
            }
        }

        public ulong MaxSize
        {
            get
            {
                return _storage.MaxSize;
            }
        }

        public Func<IStorable, bool> Conditions
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Func<IStorable, bool> Priorities
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
        {
            CollectionChanged?.Invoke(this, a);
        }

        #endregion

        #region IStorage

        #region Manipulation

        public ulong Add(IStorageSlot item, bool cancelOnOver = false)
        {
            var previous = item.Stack.Current;
            var res = _storage.Add(item, cancelOnOver);
            if (res != previous)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public ulong Add(IStorable item, ulong amount = 1, bool cancelOnOver = false)
        {
            var res = _storage.Add(item, amount, cancelOnOver);
            if (res != amount)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public ulong Remove(IStorageSlot item, bool cancelOnUnder = false)
        {
            var previous = item.Stack.Current;
            var res = _storage.Remove(item, cancelOnUnder);
            if (res != previous)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public ulong Remove(IStorable item, ulong amount = 1, bool cancelOnUnder = false)
        {
            var res = _storage.Remove(item, amount, cancelOnUnder);
            if (res != amount)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public bool Remove(int index)
        {
            var res = _storage.Remove(index);
            if (res)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        public bool Switch(IStorageSlot first, IStorageSlot second)
        {
            var res = _storage.Switch(first, second);
            if (res)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return res;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<IStorageSlot> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion

        #region Search

        public int IndexOf(IStorageSlot item)
        {
            return _storage.IndexOf(item);
        }

        #endregion

        #region Rules

        public bool CheckPriorities(IStorable storable)
        {
            return _storage.CheckPriorities(storable);
        }

        public bool CheckPriorities(IStorageSlot slot)
        {
            return _storage.CheckPriorities(slot);
        }

        public bool CheckConditions(IStorable storable)
        {
            return _storage.CheckConditions(storable);
        }

        public bool CheckConditions(IStorageSlot slot)
        {
            return _storage.CheckConditions(slot);
        }

        #endregion

        #endregion
    }
}
