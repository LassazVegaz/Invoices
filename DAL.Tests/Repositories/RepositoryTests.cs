using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CORE.Models;
using DAL.Data;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace DAL.Repositories.Tests
{
    [TestFixture()]
    [TestClass()]
    [SetUpFixture()]
    public class RepositoryTests
    {

        // Test data
        readonly Item[] data = new Item[] {
                    new Item { Id = 1 , Amount = 3 , Name = "Orange" , Price = 20 },
                    new Item { Id = 2 , Amount = 6 , Name = "Cake" , Price = 50 },
                    new Item { Id = 3 , Amount = 10 , Name = "Apple" , Price = 100 },
        };
        // DB context options
        DbContextOptions<InvoicesDBContext> options;



        [SetUp]
        public void SetDB()
        {

            // create inmemory database options
            options = new DbContextOptionsBuilder<InvoicesDBContext>()
                .UseInMemoryDatabase(databaseName: "InvoicesDB")
                .Options;

            // insert test data
            using var context = new InvoicesDBContext(options);
            context.Items.AddRange(data);
            context.SaveChanges();

        }



        [TestMethod()]
        public void InsertTest()
        {

            // test repo
            using var testContext = new InvoicesDBContext(options);
            Repository<Item, int> repo = new ItemRepository(testContext);
            var res = repo.GetAll();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i].Amount, res[i].Amount);
                Assert.AreEqual(data[i].Id, res[i].Id);
                Assert.AreEqual(data[i].Name, res[i].Name);
                Assert.AreEqual(data[i].Price, res[i].Price);
            }

        }

    }
}