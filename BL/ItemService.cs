using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Repositories;
using CORE.Services;
using CORE.UnitOfWork;

namespace BL
{
    public class ItemService : IItemService
    {
        readonly IItemRepository itemRepository;
        readonly IInvoicesContext context;

        public ItemService(IInvoicesContext context)
        {
            this.context = context;
            this.itemRepository = context.ItemRepository;
        }

        public Item CreateItem(Item item)
        {

            return itemRepository.Create(item).Entity;
            
        }

        public void DeleteItem(int itemID)
        {
            // get item
            var item = GetItem(itemID);
            // delete item
            itemRepository.Delete(item);
        }

        public Item[] GetAllItems()
        {
            return itemRepository.GetAll().ToArray();
        }

        public Item GetItem(int itemID)
        {
            return itemRepository.GetByID(itemID);
        }

        public bool ItemExists(int itemID)
        {
            return GetItem(itemID) is not null;
        }

        public int ItemRemainingAmount(int itemID)
        {

            // get all invoice items
            var allInvoiceItems = context.InvoiceItemRepository.GetByItemID(itemID);

            // sum sold amount
            var soldAmount = allInvoiceItems.Sum(II => II.ItemAmount);

            // return remaining amount
            return GetItem(itemID).Amount - soldAmount;
            
        }

        public void UpdateItem(Item item)
        {

            itemRepository.Update(item);
            
        }
    }
}
