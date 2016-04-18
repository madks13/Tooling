using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGUI.Tools
{
    public class ManaPotion : Displayable
    {
        public ManaPotion()
        {
            Name = "Small Mana Potion";
            Image = "F:\\Projects\\Tooling\\TestingGUI\\Tools\\Inventory\\img\\mana_potion.png";
            Stack = 250;
        }
    }
}
