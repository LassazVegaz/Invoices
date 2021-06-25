using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Services;
using CORE.UnitOfWork;

namespace BL
{
    public class InvoiceService : IInvoiceService
    {

        readonly IInvoicesContext context;

        public InvoiceService(IInvoicesContext context)
        {
            this.context = context;
        }

        public InvoiceWithItems CreateInvoice(InvoiceWithItems invoice)
        {
            return context.CreateInvoice(invoice);
        }

        public void DeleteInvoice(int invoiceID)
        {
            context.DeleteInvoice(invoiceID);
        }

        public InvoiceWithItems[] GetAllInvoices()
        {
            return context.GetAllInvoices().ToArray();
        }

        public InvoiceWithItems GetInvoice(int invoiceID)
        {
            return context.GetInvoice(invoiceID);
        }

        public bool InvoiceExists(int invoiceID)
        {
            return GetInvoice(invoiceID) is not null;
        }

        public InvoiceWithItems UpdateInvoice(InvoiceWithItems invoice)
        {
            return context.UpdateInvoice(invoice);
        }

    }
}
