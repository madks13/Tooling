using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGUI.Tools
{
    public class HealthPotion : Displayable
    {
        public HealthPotion()
        {
            Name = "Small Health Potion";
            Image = "F:\\Projects\\Tooling\\TestingGUI\\Tools\\Inventory\\img\\health_potion.png";
            Stack = 250;
        }
    }
}
