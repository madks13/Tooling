using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGUI.Tools
{
    public class ObservableCollection : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal void OnCollectionChanged(NotifyCollectionChangedEventArgs a)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, a);
            }
        }
    }
}
