using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Repositories;

namespace CORE.UnitOfWork
{
    public interface IInvoicesContext
    {

        IItemRepository ItemRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInvoiceItemRepository InvoiceItemRepository { get; }

        InvoiceWithItems CreateInvoice(InvoiceWithItems invoiceWithItems);
        void DeleteInvoice(int invoiceID);
        List<InvoiceWithItems> GetAllInvoices();
        InvoiceWithItems GetInvoice(int invoiceID);
        InvoiceWithItems UpdateInvoice(InvoiceWithItems invoiceWithItems);

    }
}
