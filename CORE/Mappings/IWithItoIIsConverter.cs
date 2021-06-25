using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Models;

namespace CORE.Mappings
{
    class IWithItoIIsConverter : ITypeConverter<InvoiceWithItems, InvoiceItem[]>
    {
        public InvoiceItem[] Convert(InvoiceWithItems source, InvoiceItem[] destination, ResolutionContext context)
        {
            return source
                .Items
                .ConvertAll(i => new InvoiceItem { InvoiceId = source.Id, ItemId = i.Id })
                .ToArray();
        }
    }
}
