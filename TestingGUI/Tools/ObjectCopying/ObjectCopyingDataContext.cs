using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Dynamic.ObjectCopying;
using Tools.Observable;

namespace TestingGUI.Tools.ObjectCopying
{
    [CopyableProperty(typeof(ObjectViewModel), "")]
    [CopyableProperty(typeof(Object), "")]
    public class Object : ObservableProperties
    {
        private string _name;

        [PropertyPath(typeof(ObjectViewModel), "Name")]
        [PropertyPath(typeof(Object), "Name")]
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }
        
        private int _weight;

        [PropertyPath(typeof(ObjectViewModel), "Weight")]
        [PropertyPath(typeof(Object), "Weight")]
        public int Weight
        {
            get { return _weight; }
            set { SetField(ref _weight, value); }
        }
        
        public SubObject SubObject { get; set; }
    }

    [CopyableProperty(typeof(ObjectViewModel), "")]
    [CopyableProperty(typeof(Object), "SubObject")]
    public class SubObject : ObservableProperties
    {
        private string _name;

        [PropertyPath(typeof(ObjectViewModel), "SubObjectName")]
        [PropertyPath(typeof(Object), "Name")]
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }
        
        private double _value;

        [PropertyPath(typeof(ObjectViewModel), "SubObjectValue")]
        [PropertyPath(typeof(Object), "Value")]
        public double Value
        {
            get { return _value; }
            set { SetField(ref _value, value); }
        }

    }
    
    [CopyableProperty(typeof(Object), "")]
    public class ObjectViewModel : ObservableProperties
    {
        private string _name;

        [PropertyPath(typeof(Object), "Name")]
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        private int _weight;

        [PropertyPath(typeof(Object), "Weight")]
        public int Weight
        {
            get { return _weight; }
            set { SetField(ref _weight, value); }
        }

        private string _subObjectName;
        [PropertyPath(typeof(Object), "SubObject.Name")]
        public string SubObjectName
        {
            get { return _subObjectName; }
            set { SetField(ref _subObjectName, value); }
        }

        private double _subObjectValue;

        [PropertyPath(typeof(SubObject), "Value")]
        //[PropertyPath(typeof(Object), "SubObject.Value")]
        public double SubObjectValue
    {
            get { return _subObjectValue; }
            set { SetField(ref _subObjectValue, value); }
        }

    }

    public class ObjectCopyingDataContext
    {
        public Boolean Direction { get; set; }

        public ObjectCopyingDataContext()
        {
            Direction = true;
            if (Direction)
            {
                ViewModel = new ObjectViewModel() { Name = "Basic object", Weight = 3, SubObjectName = "Basic Sub Object", SubObjectValue  = 4.5 };
                Object = new Object() { SubObject = new SubObject() };
                Object2 = new Object() { Name = "Basic object", Weight = 3, SubObject = new SubObject() { Name = "Basic Sub Object", Value = 4.5 } };
            }
            else
            {
                ViewModel = new ObjectViewModel();
                Object = new Object() { Name = "Basic object", Weight = 3, SubObject = new SubObject() { Name = "Basic Sub Object", Value = 4.5 } };
                Object2 = new Object() { SubObject = new SubObject() };
            }
        }

        public ObjectViewModel ViewModel { get; set; }

        public Object Object { get; set; }

        public Object Object2 { get; set; }

    }

}
