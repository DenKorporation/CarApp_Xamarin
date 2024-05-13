using System.Collections.ObjectModel;
using CarApp.Models;

namespace CarApp.Abstractions
{
    public interface ICarsService
    {
        ObservableCollection<Car> Cars { get; }
    }
}