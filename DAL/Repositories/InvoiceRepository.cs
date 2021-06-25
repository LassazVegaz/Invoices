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
    public class InvoiceRepository : Repository<Invoice, int>, IInvoiceRepository
    {
        public InvoiceRepository(InvoicesDBContext context) : base(context)
        {
        }

        public new List<Invoice> GetAll()
        {
            return _context.Invoices.AsNoTracking()
                                    .Include(i => i.InvoiceItems)
                                    .ToList();
        }
    }
}
