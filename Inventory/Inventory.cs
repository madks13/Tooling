using Inventory.Interfaces;
using System;

namespace Inventory
{
    public class Inventory
    {
        private IStorage _storage;
        
        public Inventory(IStorage storage)
        {
            _storage = storage;
        }

        public UInt64 Add(IStorable item)
        {
            //var left = item.Stack;
            //var list = _storage.Items.Where(i => i.Item.IsSameType(item));

            //foreach (var element in list)
            //{
            //    left = element.Add(left);
            //    if (left <= 0)
            //    {
            //        break;
            //    }
            //}

            //if (left > 0)
            //{
            //    //Try to add what's left to the storage
            //    if (_storage.Add(item))
            //    {
            //        left = 0;
            //    }
            //}

            //return left;
            return 0;
        }
    }
}
