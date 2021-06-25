using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Models;

namespace CORE.Services
{
    public interface IInvoiceService
    {
        InvoiceWithItems[] GetAllInvoices();
        InvoiceWithItems GetInvoice(int invoiceID);
        InvoiceWithItems CreateInvoice(InvoiceWithItems invoice);
        void DeleteInvoice(int invoiceID);
        InvoiceWithItems UpdateInvoice(InvoiceWithItems invoice);
        bool InvoiceExists(int invoiceID);
    }
}
