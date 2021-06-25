using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class InvoiceItem
    {
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}
