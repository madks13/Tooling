using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Tools.Observable;

namespace TestingGUI.Tools
{
    public class MainDataContext : ObservableProperties
    {
        private List<Page> _testingPages = new List<Page>() { new Pages.Grading(), new Pages.Inventory(), new Pages.Stats() };
        
        public List<Page> Pages
        {
            get
            {
                return _testingPages.ToList();
            }
        }
    }
}
