using Grading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestingGUI.Tools;

namespace TestingGUI.Pages
{
    /// <summary>
    /// Interaction logic for Grading.xaml
    /// </summary>
    public partial class Grading : Page, INotifyPropertyChanged
    {
        private GradingDataContext _dataContext = new GradingDataContext();

        public Grading()
        {
            Title = "Grading";
            InitializeComponent();
            DataContext = _dataContext;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public new String Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public new Brush Background
        {
            get
            {
                return base.Background;
            }
            set
            {
                base.Background = value;
                OnPropertyChanged("Background");
            }
        }

        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CategoryToAddTo.AddSubCategory(new Category(_dataContext.NewCategoryName,
                new Interval(_dataContext.NewCategoryMinGrade, _dataContext.NewCategoryMaxGrade),
                new Interval(_dataContext.NewCategoryMinWeight, _dataContext.NewCategoryMaxWeight),
                _dataContext.NewCategoryWeight));
            _dataContext.CategoryToAddTo = null;
            NewCategoryForm.Visibility = Visibility.Hidden;
        }

        private void ShowNewCategoryFormButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            _dataContext.CategoryToAddTo = b.DataContext as Category;
            NewCategoryForm.Visibility = Visibility.Visible;
        }

        }
    }
