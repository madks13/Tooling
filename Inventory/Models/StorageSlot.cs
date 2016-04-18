using Inventory.Interfaces;
using System;
using Tools.Observable;

namespace Inventory.Models
{
    public class StorageSlot : ObservableProperties, IStorageSlot
    {
        #region Fields

        private IStorable _item;
        private IStack _stack;
        
        #endregion

        #region Properties

        public IStorable Item
        {
            get
            {
                return _item;
            }
            set
            {
                if (SetField(ref _item, value))
                {
                    UpdateItem();
                }
            }
        }

        public IStack Stack
        {
            get { return _stack; }
        }
        
        #endregion

        #region C/Dtor

        public StorageSlot(IStorable item, UInt64 amount = 1)
        {
            _stack = new Stack();
            _stack.PropertyChanged += _stack_PropertyChanged;
            Item = item;
            if (Item != null)
            {
                Item.PropertyChanged += _item_PropertyChanged;
                Stack.Current = amount;
            }
        }

        #endregion

        #region Events

        public event EventHandler ItemSwitched;
        public event EventHandler ItemChanged;

        private void OnItemSwitched()
        {
            ItemSwitched?.Invoke(this, null);
        }

        private void OnItemChanged()
        {
            ItemChanged?.Invoke(this, null);
        }

        #endregion

        #region Event listeners

        private void _stack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Current":
                    if (_stack.Current == 0)
                    {
                        SetItem(null, 0);
                    }
                    break;
                default:
                    break;
            }
        }

        private void _item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Stack":
                    Stack.Max = Item.Stack;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods

        #region Private

        private void UpdateItem(UInt64 amount = 1)
        {
            if (Item != null)
            {
                Stack.Max = Item.Stack;
                Stack.Current = amount;
            }
            else
            {
                Stack.Max = 0;
                Stack.Current = 0;
            }
        }
        
        #endregion

        #region Public

        public void SetItem(IStorable item, UInt64 amount)
        {
            IStorable previous = Item;

            Item = item;
            UpdateItem(amount);

            if (previous != null && item != null)
            {
                OnItemSwitched();
            }
            else
            {
                OnItemChanged();
            }
        }

        public UInt64 Add(UInt64 amount, Boolean cancelOnOver = false)
        {
            return _stack.Add(amount, cancelOnOver);
        }

        public UInt64 Remove(UInt64 amount, Boolean cancelOnUnder = false)
        {
            return _stack.Remove(amount, cancelOnUnder);
        }
        
        #endregion

        #endregion
    }
}
