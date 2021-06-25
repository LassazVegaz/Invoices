using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new List<InvoiceItem>();
        }

        public int Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedFor { get; set; }

        public virtual List<InvoiceItem> InvoiceItems { get; set; }
    }
}
