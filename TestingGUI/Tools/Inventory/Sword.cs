using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGUI.Tools
{
    public class Sword : Displayable
    {
        public Sword()
        {
            Name = "Training Sword";
            Image = "F:\\Projects\\Tooling\\TestingGUI\\Tools\\Inventory\\img\\sword.png";
            Stack = 1;
        }
    }
}
