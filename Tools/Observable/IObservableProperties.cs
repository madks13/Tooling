using System.ComponentModel;

namespace Tools.Observable
{
    public interface IObservableProperties
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}