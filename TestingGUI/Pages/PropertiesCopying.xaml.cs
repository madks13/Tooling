using System;
using System.Collections.Generic;
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
using Tools.Dynamic.ObjectCopying;

namespace TestingGUI.Pages
{
    /// <summary>
    /// Interaction logic for PropertiesCopying.xaml
    /// </summary>
    public partial class PropertiesCopying : Page
    {
        private TestingGUI.Tools.ObjectCopying.ObjectCopyingDataContext _objectCopyingDataContext = new TestingGUI.Tools.ObjectCopying.ObjectCopyingDataContext();

        public PropertiesCopying()
        {
            InitializeComponent();
            DataContext = _objectCopyingDataContext;
        }
        
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (_objectCopyingDataContext.Direction)
            {
                //ObjectCopying.CopyProperties(_objectCopyingDataContext.Object, _objectCopyingDataContext.ViewModel, "");
                ObjectCopying.CopyProperties(_objectCopyingDataContext.Object, _objectCopyingDataContext.Object2);
            }
            else
            {
                //ObjectCopying.CopyProperties(_objectCopyingDataContext.ViewModel, _objectCopyingDataContext.Object, "");
                ObjectCopying.CopyProperties(_objectCopyingDataContext.Object2, _objectCopyingDataContext.Object);
            }
        }

        private void Direction_Click(object sender, RoutedEventArgs e)
        {
            if (_objectCopyingDataContext.Direction)
            {
                var to = _objectCopyingDataContext.Object;
                to.Name = String.Empty;
                to.Weight = 0;
                to.SubObject.Name = String.Empty;
                to.SubObject.Value = 0;

                var from = _objectCopyingDataContext.ViewModel;
                from.Name = "Basic object";
                from.Weight = 3;
                from.SubObjectName = "Basic Sub Object";
                from.SubObjectValue = 4.5;

                var from2 = _objectCopyingDataContext.Object2;
                from2.Name = "Basic object";
                from2.Weight = 3;
                from2.SubObject.Name = "Basic Sub Object";
                from2.SubObject.Value = 4.5;
            }   
            else
            {
                var from = _objectCopyingDataContext.Object;
                from.Name = "Basic object"; 
                from.Weight = 3; 
                from.SubObject.Name = "Basic Sub Object"; 
                from.SubObject.Value = 4.5; 

                var to = _objectCopyingDataContext.ViewModel;
                to.Name = String.Empty;
                to.Weight = 0;
                to.SubObjectName = String.Empty;
                to.SubObjectValue = 0;

                var to2 = _objectCopyingDataContext.Object2;
                to2.Name = String.Empty;
                to2.Weight = 0;
                to2.SubObject.Name = String.Empty;
                to2.SubObject.Value = 0;
            }

            _objectCopyingDataContext.Direction = !_objectCopyingDataContext.Direction;
        }
    }
}
