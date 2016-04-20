using Inventory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Observable;
using System.Collections.Specialized;

namespace Inventory.Models
{
    public abstract class Storage : ObservableProperties, IStorage
    {
        #region Fields

        protected Func<IStorable, Boolean> _priorities;
        protected Func<IStorable, Boolean> _conditions;
        protected IEnumerable<IStorageSlot> _storage;

        #endregion

        #region C/Dtor

        public Storage(Func<IStorable, Boolean> conditions = null, Func<IStorable, Boolean> priorities = null)
        {
            Priorities = priorities;
            Conditions = conditions;
        }

        #endregion

        #region Properties

        public abstract ulong FreeSlots { get; set; }

        public abstract ulong MaxSize { get; set; }

        public Func<IStorable, bool> Conditions
        {
            get
            {
                return _conditions;
            }
            set
            {
                SetField(ref _conditions, value);
            }
        }

        public Func<IStorable, bool> Priorities
        {
            get
            {
                return _priorities;
            }
            set
            {
                SetField(ref _priorities, value);
            }
        }

        #endregion

        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
        {
            CollectionChanged?.Invoke(this, a);
        }

        #endregion

        #region Conditions

        public bool CheckConditions(IStorageSlot slot)
        {
            if (slot.Item == null)
            {
                return false;
            }
            return CheckConditions(slot.Item);
        }

        public bool CheckConditions(IStorable storable)
        {
            if (Conditions == null)
            {
                return true;
            }
            return _priorities.GetInvocationList().All(x => (bool)x.Method.Invoke(x.Target, new object[] { storable }));
        }

        #endregion

        #region Priorities

        public bool CheckPriorities(IStorageSlot slot)
        {
            if (slot.Item == null)
            {
                return false;
            }
            return CheckPriorities(slot.Item);
        }

        public bool CheckPriorities(IStorable storable)
        {
            if (Priorities == null)
            {
                return true;
            }
            return _priorities.GetInvocationList().Any(x => (bool)x.Method.Invoke(x.Target, new object[] { storable }));
        }

        #endregion

        #region IEnumerable<IStorageSlot> interface

        public IEnumerator<IStorageSlot> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (IStorageSlot item in _storage)
            {
                yield return item;
            }
        }

        #endregion

        #region Abstract
        
        public abstract ulong Add(IStorageSlot item, bool cancelOnOver = false);

        public abstract ulong Add(IStorable item, ulong amount = 1, bool cancelOnOver = false);

        public abstract int IndexOf(IStorageSlot item);

        public abstract bool Remove(int index);

        public abstract ulong Remove(IStorageSlot item, bool cancelOnUnder = false);

        public abstract ulong Remove(IStorable item, ulong amount = 1, bool cancelOnUnder = false);

        public abstract bool Switch(IStorageSlot first, IStorageSlot second);

        #endregion
    }
}
