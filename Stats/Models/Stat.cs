using Stats.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Observable;

namespace Stats.Models
{
    public abstract class Stat<T> : ObservableProperties, IStat<T>
    {
        #region Fields

        private String _name;
        private T _baseValue;
        private T _currentValue;
        protected ObservableCollection<IStatModifier<T>> _modifiers = new ObservableCollection<IStatModifier<T>>();

        #endregion

        #region C/Dtor

        public Stat()
        {
            _modifiers.CollectionChanged += _modifiers_CollectionChanged;
        }

        #endregion

        #region Event listeners

        private void _modifiers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<StatModifier<T>> list = sender as ObservableCollection<StatModifier<T>>;

            T tmp = Current;
            foreach (StatModifier<T> item in e.NewItems)
            {
                tmp = item.Apply(tmp);
            }

            Current = tmp;
        }

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public T Base
        {
            get { return _baseValue; }
            set { SetField(ref _baseValue, value); }
        }

        public T Current
        {
            get { return _currentValue; }
            set { SetField(ref _currentValue, value);}
        }

        public List<IStatModifier<T>> Modifiers
        {
            get { return _modifiers.ToList(); }
        }

        public Boolean AddModifier(IStatModifier<T> modifier)
        {
            if (modifier.Apply != null && modifier.Undo != null)
            {
                _modifiers.Add(modifier);
                return true;
            }
            return false;
        }
        #endregion
    }
}
