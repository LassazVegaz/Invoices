using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Repositories;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class InvoiceItemRepository : Repository<InvoiceItem, int[]>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(InvoicesDBContext context) : base(context)
        {
        }

        public InvoiceItem GetByID(int invoiceID, int itemID)
        {
            return _context.InvoiceItems.Find(invoiceID, itemID);
        }

        public InvoiceItem[] GetByID(int invoiceID)
        {
            return _context.InvoiceItems
                .AsNoTracking()
                .Include(II=>II.Item)
                .Include(II=>II.Invoice)
                .Where(II => II.InvoiceId == invoiceID)
                .ToArray();
        }

        public InvoiceItem[] GetByItemID(int itemID)
        {
            return _context.InvoiceItems
                .AsNoTracking()
                .Include(II => II.Item)
                .Include(II => II.Invoice)
                .Where(II => II.ItemId == itemID)
                .ToArray();
        }
    }
}
