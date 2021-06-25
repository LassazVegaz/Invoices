using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Models;

namespace CORE.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Invoice, InvoiceWithItems>();

            CreateMap<InvoiceWithItems, Invoice>();

            CreateMap<SoldItem, Item>();

            CreateMap<InvoiceItem, SoldItem>()
                .ForMember(i => i.Id, o => o.MapFrom(i => i.ItemId))
                .ForMember(i => i.Name, o => o.MapFrom(i => i.Item.Name))
                .ForMember(i => i.Price, o => o.MapFrom(i => i.Item.Price))
                .ForMember(i => i.SoldAmount, o => o.MapFrom(i => i.ItemAmount));

            CreateMap<InvoiceWithItems, InvoiceItem[]>().ConvertUsing<IWithItoIIsConverter>();

        }
    }
}
