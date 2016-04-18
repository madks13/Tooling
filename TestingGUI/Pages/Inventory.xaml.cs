using Inventory;
using Inventory.Interfaces;
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
using TestingGUI.Tools;

namespace TestingGUI.Pages
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {
        private InventoryDataContext _inventoryDataContext = new InventoryDataContext(5);

        private object dragedItem = null;
        private object origin = null;

        public Inventory()
        {
            InitializeComponent();
            DataContext = _inventoryDataContext;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            IStorageSlot it = lv.SelectedItem as IStorageSlot;
        }

        private void CleanTmp()
        {
            dragedItem = null;
            origin = null;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var item = ChoicesList.SelectedItem as IDisplayable;
            var newItem = Activator.CreateInstance(item.GetType()) as IDisplayable;
            var selectedSlot = InventoryItems.SelectedItem as IStorageSlot;
            _inventoryDataContext.Items.Add(newItem, _inventoryDataContext.Quantity);
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryItems.SelectedItem != null)
            {
                _inventoryDataContext.Items.Remove(_inventoryDataContext.Items.IndexOf(InventoryItems.SelectedItem as IStorageSlot));
            }
        }

        private void DeleteItem2_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryItems2.SelectedItem != null)
            {
                _inventoryDataContext.Items2.Remove(_inventoryDataContext.Items2.IndexOf(InventoryItems2.SelectedItem as IStorageSlot));
            }
        }

        private void InventoryItems_Drop(object sender, DragEventArgs e)
        {
            if (dragedItem != null)
            {
                ListView o = origin as ListView;
                ListView s = sender as ListView;

                IStorage originInventory = o.Items.SourceCollection as IStorage;
                IStorage senderInventory = s.Items.SourceCollection as IStorage;
                    
                if (typeof(IStorageSlot).IsAssignableFrom(dragedItem.GetType()))
                {
                    IStorageSlot i = dragedItem as IStorageSlot;

                    //Save info for adding it into the target inventory
                    IStorable item = i.Item;
                    UInt64 amount = i.Stack.Current;
                    int index = originInventory.IndexOf(i);

                    //Remove the item from the original inventory
                    originInventory.Remove(index);

                    //Then put it in the target inventory
                    senderInventory.Add(item, amount);
                }
                else if (typeof(IDisplayable).IsAssignableFrom(dragedItem.GetType()))
                {
                    senderInventory.Add(dragedItem as IDisplayable, _inventoryDataContext.Quantity);
                }

                CleanTmp();
            }
        }

        private void InventoryItem_Drop(object sender, DragEventArgs e)
        {
            if (dragedItem != null)
            {
                FrameworkElement s = sender as FrameworkElement;
                IStorageSlot slot = s.DataContext as IStorageSlot;

                
                if (typeof(IStorageSlot).IsAssignableFrom(dragedItem.GetType()))
                {
                    _inventoryDataContext.Items.Switch(slot, dragedItem as IStorageSlot);
                }
                else if (typeof(IDisplayable).IsAssignableFrom(dragedItem.GetType()))
                {
                    IDisplayable selectedItem = dragedItem as IDisplayable;
                    if (slot.Item != null && slot.Item.IsSameType(selectedItem))
                    {
                        slot.Add(_inventoryDataContext.Quantity);
                    }
                    else
                    {
                        slot.SetItem(selectedItem, _inventoryDataContext.Quantity);
                    }
                }
                CleanTmp();
            }
        }

        private void ChoicesItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
            {
                IStorable item = ((FrameworkElement)sender).DataContext as IStorable;
                _inventoryDataContext.Items.Add(item, _inventoryDataContext.Quantity);
            }
            else if (e.ClickCount == 2)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    IStorable item = ((FrameworkElement)sender).DataContext as IStorable;
                    _inventoryDataContext.Items.Add(item, _inventoryDataContext.Quantity);
                }
            }
            else if (e.ClickCount == 1)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    FrameworkElement itemControl = sender as FrameworkElement;
                    dragedItem = itemControl.DataContext;
                }
            }
        }

        private void InventoryItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
            {
                IStorageSlot item = ((FrameworkElement)sender).DataContext as IStorageSlot;
                item.Stack.Current = _inventoryDataContext.Items2.Add(item);
            }
            else if (e.ClickCount == 2)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    IStorageSlot item = ((FrameworkElement)sender).DataContext as IStorageSlot;
                    item.Stack.Current = _inventoryDataContext.Items2.Add(item);
                }
            }
            else if (e.ClickCount == 1)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    FrameworkElement itemControl = sender as FrameworkElement;
                    dragedItem = itemControl.DataContext;
                }
            }
        }

        private void Inventory2Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
            {
                IStorageSlot item = ((FrameworkElement)sender).DataContext as IStorageSlot;
                item.Stack.Current = _inventoryDataContext.Items.Add(item);
            }
            else if (e.ClickCount == 2)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    IStorageSlot item = ((FrameworkElement)sender).DataContext as IStorageSlot;
                    item.Stack.Current = _inventoryDataContext.Items.Add(item);
                }
            }
            else if (e.ClickCount == 1)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    FrameworkElement itemControl = sender as FrameworkElement;
                    dragedItem = itemControl.DataContext;
                }
            }
        }

        private void SetMouseMoveVars(object sender, MouseEventArgs a)
        {
            if (a.LeftButton == MouseButtonState.Pressed)
            {
                origin = sender;
                DependencyObject s = sender as DependencyObject;
                DragDrop.DoDragDrop(s, dragedItem, DragDropEffects.Link);
            }
        }

        private void ChoicesItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragedItem != null)
            {
                SetMouseMoveVars(ChoicesList, e);
            }
        }

        private void InventoryItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragedItem != null)
            {
                SetMouseMoveVars(InventoryItems, e);
            }
        }

        private void Inventory2Item_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragedItem != null)
            {
                SetMouseMoveVars(InventoryItems2, e);
            }
        }

        private void Delete_Drop(object sender, DragEventArgs e)
        {
            if (dragedItem != null)
            {
                ListView o = origin as ListView;

                IStorage originInventory = o.Items.SourceCollection as IStorage;

                if (typeof(IStorageSlot).IsAssignableFrom(dragedItem.GetType()))
                {
                    IStorageSlot i = dragedItem as IStorageSlot;

                    //Save info for adding it into the target inventory
                    IStorable item = i.Item;
                    UInt64 amount = i.Stack.Current;
                    int index = originInventory.IndexOf(i);

                    //Remove the item from the original inventory
                    originInventory.Remove(index);
                }

                dragedItem = null;
                origin = null;
            }
        }
    }
}
