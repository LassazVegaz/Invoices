using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class InvoiceWithItems
    {
        public InvoiceWithItems()
        {
            Items = new List<SoldItem>();
        }

        public int Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedFor { get; set; }

        public virtual List<SoldItem> Items { get; set; }
    }
}
