using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Repositories;
using DAL.Data;

namespace DAL.Repositories
{
    public class ItemRepository : Repository<Item, int>, IItemRepository
    {
        public ItemRepository(InvoicesDBContext context) : base(context)
        {
        }
    }
}
