using NUnit.Framework;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using CORE.UnitOfWork;
using CORE.Mappings;
using DAL.UnitOfWork;
using CORE.Services;

namespace BL.Tests
{
    [TestFixture()]
    public class InvoiceServiceTests
    {

        //---- TEST DATA ----
        //invoices
        readonly Invoice[] invoices = new Invoice[] {
                    new Invoice { Id = 1 , IssuedDate=DateTime.Now , IssuedFor="Dilani" },
                    new Invoice { Id = 2 , IssuedDate=DateTime.Now , IssuedFor="Lassi"   },
        };
        //items
        readonly Item[] items = new Item[] {
                    new Item { Id = 1 , Amount = 3 , Name = "Orange" , Price = 20 },
                    new Item { Id = 2 , Amount = 6 , Name = "Cake" , Price = 50 },
                    new Item { Id = 3 , Amount = 10 , Name = "Apple" , Price = 100 },
        };
        //invoice items
        readonly InvoiceItem[] invoiceItems = new InvoiceItem[]
        {
            new InvoiceItem { InvoiceId = 1 , ItemId = 1 , ItemAmount = 1 },
            new InvoiceItem { InvoiceId = 1 , ItemId = 2 , ItemAmount = 3 },
            new InvoiceItem { InvoiceId = 1 , ItemId = 3 , ItemAmount = 5 },
            new InvoiceItem { InvoiceId = 2 , ItemId = 1 , ItemAmount = 2 },
            new InvoiceItem { InvoiceId = 2 , ItemId = 3 , ItemAmount = 3 },
        };
        // Test data
        List<InvoiceWithItems> invoiceWithItemsList;
        // DB context options
        DbContextOptions<InvoicesDBContext> options;
        // context
        IInvoicesContext context;



        [SetUp()]
        public void SetDB()
        {

            // Create mapper
            var mapperConfigurations = new MapperConfiguration(conf => conf.AddProfile(new MappingProfile()));
            var mapper = new Mapper(mapperConfigurations);

            // create inmemory database options
            options = new DbContextOptionsBuilder<InvoicesDBContext>()
                .UseInMemoryDatabase(databaseName: "InvoicesDB")
                .EnableSensitiveDataLogging()
                .Options;

            // insert test data
            var dbContext = new InvoicesDBContext(options);
            dbContext.Invoices.AddRange(invoices);
            dbContext.Items.AddRange(items);
            dbContext.InvoiceItems.AddRange(invoiceItems);
            dbContext.SaveChanges();

            // get invoice context
            context = new InvoicesContext(dbContext, mapper);

            // get invoices with items
            invoiceWithItemsList = context.GetAllInvoices();

        }

        [TearDown()]
        public void RemoveDB()
        {

            using var context = new InvoicesDBContext(options);
            context.Database.EnsureDeleted();

        }



        [Test()]
        public void CreateInvoiceTest()
        {

            // init new invoice
            var newInvoice = new InvoiceWithItems
            {
                IssuedDate = DateTime.Now,
                IssuedFor = "Champika",
                Items = new List<SoldItem>(new SoldItem[]
                {
                    new SoldItem { Id = items[0].Id , SoldAmount = 1 , Name = items[0].Name , Price = items[0].Price },
                    new SoldItem { Id = items[1].Id , SoldAmount = 1 , Name = items[1].Name , Price = items[1].Price },
                })
            };

            // test service
            IInvoiceService invoiceService = new InvoiceService(context);
            invoiceService.CreateInvoice(newInvoice);

            // get all invoices
            var allInvoices = context.GetAllInvoices();

            // there should at least one invoice same as above one
            var invoicesCount = allInvoices.Count(i => i.IssuedDate == newInvoice.IssuedDate
                                                       && i.IssuedFor == newInvoice.IssuedFor
                                                       && i.Items is not null
                                                       && i.Items.Count == newInvoice.Items.Count);

            Assert.True(invoicesCount > 0);

        }

        [Test()]
        public void DeleteInvoiceTest()
        {

            // test service
            IInvoiceService invoiceService = new InvoiceService(context);
            invoiceService.DeleteInvoice(invoiceWithItemsList[0].Id);

            // invoice should not be there
            Assert.Null(context.GetInvoice(invoiceWithItemsList[0].Id));

        }

        [Test()]
        public void GetAllInvoicesTest()
        {

            // test service
            IInvoiceService invoiceService = new InvoiceService(context);
            var serviceInvoices = invoiceService.GetAllInvoices();

            // check if each invoice is in the test data set
            foreach (var item in serviceInvoices)
            {

                // there should be only one invoice same as above one
                var invoicesCount = invoiceWithItemsList.Count(i => i.IssuedDate == item.IssuedDate
                                                                    && i.IssuedFor == item.IssuedFor
                                                                    && i.Items is not null
                                                                    && i.Items.Count == item.Items.Count);

                Assert.True(invoicesCount == 1);

            }

        }

        [Test()]
        public void GetInvoiceTest()
        {

            // test service
            IInvoiceService invoiceService = new InvoiceService(context);
            var serviceInvoice = invoiceService.GetInvoice(invoiceWithItemsList[0].Id);

            // returned one should be equal to existing one
            Assert.AreEqual(invoiceWithItemsList[0].Id, serviceInvoice.Id);
            Assert.AreEqual(invoiceWithItemsList[0].IssuedDate, serviceInvoice.IssuedDate);
            Assert.AreEqual(invoiceWithItemsList[0].IssuedFor, serviceInvoice.IssuedFor);
            Assert.NotNull(serviceInvoice.Items);
            Assert.True(serviceInvoice.Items.Count == invoiceWithItemsList[0].Items.Count);

        }

        [Test()]
        public void UpdateInvoiceTest()
        {

            // change test data
            invoiceWithItemsList[0].IssuedFor = "Changed";

            // test service
            IInvoiceService invoiceService = new InvoiceService(context);
            invoiceService.UpdateInvoice(invoiceWithItemsList[0]);

            // get selected one from database
            var invoice = context.GetInvoice(invoiceWithItemsList[0].Id);

            // check if values are updated
            Assert.AreEqual(invoiceWithItemsList[0].Id,         invoice.Id);
            Assert.AreEqual(invoiceWithItemsList[0].IssuedDate, invoice.IssuedDate);
            Assert.AreEqual(invoiceWithItemsList[0].IssuedFor, invoice.IssuedFor);
            Assert.NotNull(invoice.Items);
            Assert.True(invoice.Items.Count == invoiceWithItemsList[0].Items.Count);

        }
    }
}