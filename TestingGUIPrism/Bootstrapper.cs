using Microsoft.Practices.Unity;
using Prism.Unity;
using TestingGUIPrism.Views;
using System.Windows;

namespace TestingGUIPrism
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
