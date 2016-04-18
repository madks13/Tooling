using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory;
using Inventory.Interfaces;
using Tools.Observable;

namespace TestingGUI.Tools
{
    public abstract class Displayable : ObservableProperties, IDisplayable
    {
        private string _image;
        private string _name;
        private ulong _stack;

        public Boolean CanStack
        {
            get
            {
                return Stack > 1;
            }
        }

        public String Image
        {
            get { return _image; }
            set { SetField(ref _image, value); }
        }

        public String Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public UInt64 Stack
        {
            get { return _stack; }
            set
            {
                SetField(ref _stack, value);
                OnPropertyChanged("CanStack");
            }
        }

        public virtual Boolean IsSameType(Object item)
        {
            return item.GetType() == GetType();   
        }
    }
}
