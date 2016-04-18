using Inventory;
using Inventory.Interfaces;

namespace TestingGUI.Tools
{
    public interface IDisplayable : IStorable
    {
        string Image { get; set; }

        string Name { get; set; }
    }
}