using Inventory.Interfaces;
using System;
using Tools.Observable;

namespace Inventory.Models
{
    public abstract class Storable : ObservableProperties, IStorable
    {
        #region Fields

        private UInt64 _stack;

        #endregion

        #region Properties

        public Boolean CanStack
        {
            get
            {
                return Stack > 1;
            }
        }

        public UInt64 Stack
        {
            get
            {
                return _stack;
            }

            set
            {
                if (SetField(ref _stack, value))
                {
                    OnPropertyChanged(nameof(CanStack));
                }

            }
        }

        #endregion

        #region Methods

        public virtual Boolean IsSameType(Object item)
        {
            return item.GetType() == this.GetType();
        }

        #endregion
    }
}
