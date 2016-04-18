using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grading
{

    #region Events related

    public enum ChangedProperty
    {
        Unknown = 0,
        Weight = 1,
        Grade = 2
    }

    public class CategoryValueChangedEventArgs : EventArgs
    {
        public ChangedProperty ChangedProperty { get; set; }

        public Double NewValue { get; set; }
    }

    #endregion

    public class Category
    {
        #region Fields

        private String _name;
        private Interval _minMaxGrade;
        private Interval _minMaxWeight;
        private Double _grade;
        private Double _weight;
        private List<Category> _subCategories = new List<Category>();

        #endregion

        #region Properties

        public Double Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = _minMaxGrade.GetValue(value);
                OnGradeChanged();
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public Double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = _minMaxWeight.GetValue(value);
                OnWeightChanged();
            }
        }

        public Interval MinMax
        {
            get
            {
                return _minMaxGrade;
            }
        }

        public List<Category> SubCategories
        {
            get
            {
                return _subCategories.ToList();
            }
        }

        public Boolean IsLeaf
        {
            get { return _subCategories.Count == 0; }
        }

        #endregion

        #region Events

        public event EventHandler GradeChanged;

        #endregion

        #region Methods

        #region C/Dtor

        public Category(String name,  Interval minMaxGrade, Interval minMaxWeight, Double weight = 1.0)
        {
            _name = name;
            _minMaxGrade = minMaxGrade;
            _minMaxWeight = minMaxWeight;
            _weight = weight;
        }

        #endregion

        #region Events related

        private void PropertyChanged(CategoryValueChangedEventArgs a)
        {
            if (GradeChanged != null)
            {
                GradeChanged(this, a);
            }
        }

        private void OnWeightChanged()
        {
            PropertyChanged(new CategoryValueChangedEventArgs() { NewValue = _weight, ChangedProperty = ChangedProperty.Weight});
        }

        private void OnGradeChanged()
        {
            PropertyChanged(new CategoryValueChangedEventArgs() { NewValue = Grade, ChangedProperty = ChangedProperty.Grade});
        }

        private void SubCategory_GradeChanged(object sender, EventArgs e)
        {
            CalculateGradeFromSubCategories();
        }

        #endregion

        public Boolean AddSubCategory(Category subCategory)
        {
            if (_subCategories.Where(c => c.Name == subCategory.Name).Count() > 0)
                return false;
            subCategory.GradeChanged += SubCategory_GradeChanged;
            _subCategories.Add(subCategory);
            CalculateGradeFromSubCategories();
            OnGradeChanged();
            return true;
        }

        private void CalculateGradeFromSubCategories()
        {
            Double total = 0.0;
            Double totalWeght = 0.0;

            foreach(var subCategory in _subCategories)
            {
                total += subCategory.Grade * subCategory.Weight;
                totalWeght += subCategory.Weight;
            }

            Grade = total / totalWeght;
        }

        #endregion
    }
}
