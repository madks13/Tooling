using Grading;
using Inventory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Observable;

namespace TestingGUI.Tools
{
    public class GradingDataContext : ObservableProperties
    {
        private ObservableCollection<Category> _grades = new ObservableCollection<Category>();

        public GradingDataContext()
        {
            var c = new Category("All", new Interval(0, 100), new Interval(0, 10));
            c.GradeChanged += C_GradeChanged;
            _grades.Add(c);

            var sc = new Category("General", new Interval(0, 100), new Interval(0, 10));
            {
                c.AddSubCategory(sc);
                {
                    sc.AddSubCategory(new Category("Manual/Readme", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Copy/Pastable", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Small", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Files organization", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Code sanity", new Interval(0, 100), new Interval(0, 10)));
                }
            }
            sc = new Category("Pages behavior", new Interval(0, 100), new Interval(0, 10));
            {
                c.AddSubCategory(sc);
                {
                    sc.AddSubCategory(new Category("Index", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Home", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Recipes", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Recipes_details", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Recipes_new", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Ingredients", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Community", new Interval(0, 100), new Interval(0, 10)));
                    sc.AddSubCategory(new Category("Community_details", new Interval(0, 100), new Interval(0, 10)));
                }
            }
            sc = new Category("Comprehension", new Interval(0, 100), new Interval(0, 10));
            {
                c.AddSubCategory(sc);
                var ssc = new Category("General", new Interval(0, 100), new Interval(0, 10));
                {
                    sc.AddSubCategory(ssc);
                    {
                        ssc.AddSubCategory(new Category("Project", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Requirements", new Interval(0, 100), new Interval(0, 10)));
                    }
                }
                ssc = new Category("AngularJs", new Interval(0, 100), new Interval(0, 10));
                {
                    sc.AddSubCategory(ssc);
                    {
                        ssc.AddSubCategory(new Category("Modules", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Routing", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Controlers", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Directives", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Services", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Factories", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Cookies", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Filters", new Interval(0, 100), new Interval(0, 10)));
                        ssc.AddSubCategory(new Category("Templates", new Interval(0, 100), new Interval(0, 10)));
                    }
                }
            }
        }

        private void C_GradeChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Grades");
        }

        public ObservableCollection<Category> Grades
        {
            get { return _grades; }
            set
            {
                _grades = value;
                OnPropertyChanged("Grades");
            }
        }

        public Category CategoryToAddTo { get; set; }

        public String NewCategoryName { get; set; } = "New category name";

        public Double NewCategoryMinGrade { get; set; } = 0.0;

        public Double NewCategoryMaxGrade { get; set; } = 100.0;

        public Double NewCategoryMinWeight { get; set; } = 0.0;

        public Double NewCategoryMaxWeight { get; set; } = 100.0;

        public Double NewCategoryWeight { get; set; } = 1.0;
    }
}
