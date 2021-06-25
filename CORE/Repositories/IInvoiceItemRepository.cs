using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;

namespace CORE.Repositories
{

    public interface IInvoiceItemRepository : IRepository<InvoiceItem, int[]>
    {

        public InvoiceItem GetByID(int invoiceID, int itemID);

        public InvoiceItem[] GetByID(int invoiceID);

        public InvoiceItem[] GetByItemID(int itemID);

    }

}
