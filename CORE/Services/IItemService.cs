using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;

namespace CORE.Services
{
    public interface IItemService
    {
        Item[] GetAllItems();
        Item GetItem(int itemID);
        Item CreateItem(Item item);
        void DeleteItem(int itemID);
        void UpdateItem(Item item);
        int ItemRemainingAmount(int itemID);
        bool ItemExists(int itemID);
    }
}
