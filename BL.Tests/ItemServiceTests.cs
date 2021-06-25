using NUnit.Framework;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.UnitOfWork;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using AutoMapper;
using CORE.Mappings;
using CORE.Repositories;
using CORE.Services;

namespace BL.Tests
{
    [TestFixture()]
    public class ItemServiceTests
    {

        // Test data
        Item[] items = new Item[]
        {
            new Item { Id = 1 , Amount = 3 , Name = "Orange" , Price = 20 },
            new Item { Id = 2 , Amount = 6 , Name = "Cake" , Price = 50 },
            new Item { Id = 3 , Amount = 10 , Name = "Apple" , Price = 100 },
        };
        // invoices context
        IInvoicesContext context;
        // db context
        InvoicesDBContext DBContext;

        [SetUp()]
        public void SetData()
        {

            // create inmemory database
            var options = new DbContextOptionsBuilder<InvoicesDBContext>()
                .UseInMemoryDatabase(databaseName: "InvoicesDB")
                .Options;
            DBContext = new InvoicesDBContext(options);
            DBContext.Items.AddRange(items);
            DBContext.SaveChanges();

            // Create mapper
            var mapperConfigurations = new MapperConfiguration(conf => conf.AddProfile(new MappingProfile()));
            var mapper = new Mapper(mapperConfigurations);

            // create context
            context = new InvoicesContext(DBContext, mapper);

        }

        [TearDown()]
        public void RemoveData()
        {

            DBContext.Database.EnsureDeleted();

        }

        [Test()]
        public void CreateItemTest()
        {

            // create new item
            var item = new Item
            {
                Name = "Pine Apple",
                Amount = 100,
                Price = 50.0M
            };

            // test service
            IItemService itemService = new ItemService(context);
            itemService.CreateItem(item);

            // get inserted data
            var insertedData = context.ItemRepository.GetAll();
            // there should be only one result
            Item insertedItem = null;
            Assert.DoesNotThrow(() =>
            {
                insertedItem = insertedData.Single(i => i.Amount == item.Amount
                                                        && i.Name == item.Name
                                                        && i.Price == item.Price);
            });
            Assert.NotNull(insertedItem);

        }

        [Test()]
        public void DeleteItemTest()
        {

            // test service
            IItemService itemService = new ItemService(context);
            itemService.DeleteItem(items[0].Id);

            // get available items
            var availableItems = context.ItemRepository.GetAll();
            // there should not be delted item
            var itemExists = availableItems.Any(i => i.Amount == items[0].Amount
                                                     && i.Name == items[0].Name
                                                     && i.Price == items[0].Price
                                                     && i.Id == items[0].Id);
            Assert.False(itemExists);

        }

        [Test()]
        public void GetAllItemsTest()
        {

            // test service
            IItemService itemService = new ItemService(context);
            var serviceItems = itemService.GetAllItems();


            // check if each item exists
            foreach (var item in items)
            {

                var itemsCount = serviceItems.Count(i => i.Amount == item.Amount
                                                         && i.Name == item.Name
                                                         && i.Price == item.Price
                                                         && i.Id == item.Id);
                Assert.AreEqual(1, itemsCount);

            }

        }

        [Test()]
        public void GetItemTest()
        {

            // test service
            IItemService itemService = new ItemService(context);
            var serviceItem = itemService.GetItem(items[0].Id);

            // check if returned item is correct
            Assert.AreEqual(items[0].Id, serviceItem.Id);
            Assert.AreEqual(items[0].Name, serviceItem.Name);
            Assert.AreEqual(items[0].Price, serviceItem.Price);
            Assert.AreEqual(items[0].Amount, serviceItem.Amount);

        }

        [Test()]
        public void UpdateItemTest()
        {
            // change test data
            items[0].Name = "Changed";

            // test service
            IItemService itemService = new ItemService(context);
            itemService.UpdateItem(items[0]);

            // get item
            var updatedItem = context.ItemRepository.GetByID(items[0].Id);

            // check if returned item is correct
            Assert.AreEqual(items[0].Id, updatedItem.Id);
            Assert.AreEqual(items[0].Name, updatedItem.Name);
            Assert.AreEqual(items[0].Price, updatedItem.Price);
            Assert.AreEqual(items[0].Amount, updatedItem.Amount);
        }

        [Test()]
        public void ItemRemainingAmountTest()
        {

            // create invoice
            context.CreateInvoice(new InvoiceWithItems
            {
                IssuedDate = DateTime.Now,
                IssuedFor = "John",
                Items = new List<SoldItem>(new SoldItem[]
                {
                    new SoldItem
                    {
                        Id = items[0].Id , Name = items[0].Name , Price = items[0].Price , SoldAmount = 1
                    }
                })
            });

            // test service
            IItemService itemService = new ItemService(context);
            var remainingAmount = itemService.ItemRemainingAmount(items[0].Id);

            // get sold amount
            var allInvoices = context.GetAllInvoices();
            var soldAmount = 0;
            foreach (var invoice in allInvoices)
            {
                soldAmount += invoice
                    .Items
                    .Sum(i => i.Id == items[0].Id ? i.SoldAmount : 0);
            }

            // get total items amount
            var totalItemAmount = context.ItemRepository.GetByID(items[0].Id).Amount;

            // calculate remaining amount
            var calculatedRemainingAmount = totalItemAmount - soldAmount;

            // check count
            Assert.AreEqual(calculatedRemainingAmount, remainingAmount);

        }
    }
}