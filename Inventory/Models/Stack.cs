using Inventory.Interfaces;
using System;
using Tools.Observable;

namespace Inventory.Models
{
    public class Stack : ObservableProperties, IStack
    {
        private ulong _current;
        private ulong _max;

        public UInt64 Current
        {
            get { return _current; }
            set
            {
                if (SetField(ref _current, (value > _max? _max : (value < 0 ? 0 : value))))
                {
                    OnPropertyChanged(nameof(Left));
                }
            }
        }
        
        public UInt64 Max
        {
            get { return _max; }
            set
            {
                if (SetField(ref _max, (value < 0 ? 0 : value)))
                {
                    if (_max < Current)
                    {
                        Current = _max;
                    }
                    OnPropertyChanged(nameof(Left));
                }
            }
        }
        
        public UInt64 Left
        {
            get
            {
                return Max - Current;
            }
        }
        
        public UInt64 Add(UInt64 amount, Boolean cancelOnOver = false)
        {
            if (amount > Left)
            {
                UInt64 left;
                if (cancelOnOver)
                {
                    left = amount;
                }
                else
                {
                    left = amount - Left;
                    Current = Max;
                }

                return left;
            }

            Current += amount;

            return 0;
        }

        public UInt64 Remove(UInt64 amount, Boolean cancelOnUnder = false)
        {
            if (amount > Current)
            {
                UInt64 left;
                if (cancelOnUnder)
                {
                    left = amount;
                }
                else
                {
                    left = amount - Current;
                    Current = 0;
                }

                return left;
            }

            Current -= amount;

            return 0;
        }
    }
}
