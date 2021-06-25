using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Models;
using CORE.Repositories;
using CORE.UnitOfWork;
using DAL.Data;
using DAL.Repositories;

namespace DAL.UnitOfWork
{
    public class InvoicesContext : IInvoicesContext
    {

        private readonly InvoicesDBContext _context;
        private readonly IMapper _mapper;

        public IItemRepository ItemRepository { get; private set; }

        public IInvoiceRepository InvoiceRepository { get; private set; }

        public IInvoiceItemRepository InvoiceItemRepository { get; private set; }

        public InvoicesContext(InvoicesDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            ItemRepository = new ItemRepository(_context);
            InvoiceRepository = new InvoiceRepository(_context);
            InvoiceItemRepository = new InvoiceItemRepository(_context);
        }

        public List<InvoiceWithItems> GetAllInvoices()
        {

            var allInvoiceItems = InvoiceRepository.GetAll();

            List<InvoiceWithItems> invoicesList = allInvoiceItems.ConvertAll(i =>
            {
                var invoiceWithItem = _mapper.Map<InvoiceWithItems>(i);
                invoiceWithItem.Items = i.InvoiceItems.ConvertAll(invItem =>
                {
                    var item = ItemRepository.GetByID(invItem.ItemId);
                    return _mapper.Map<SoldItem>(invItem);
                });
                return invoiceWithItem;
            });

            return invoicesList;

        }

        public InvoiceWithItems CreateInvoice(InvoiceWithItems invoiceWithItems)
        {

            // add invoice first
            var invoice = _mapper.Map<Invoice>(invoiceWithItems);
            var newInvoice = InvoiceRepository.Create(invoice);

            // add invoice id
            invoiceWithItems.Id = newInvoice.Entity.Id;

            // add relationship data
            var invoiceItems = _mapper.Map<InvoiceItem[]>(invoiceWithItems);
            foreach (var II in invoiceItems)
                InvoiceItemRepository.Create(II);

            return invoiceWithItems;

        }

        public void DeleteInvoice(int invoiceID)
        {

            var invoice = InvoiceRepository.GetByID(invoiceID);
            InvoiceRepository.Delete(invoice);

        }

        public InvoiceWithItems GetInvoice(int invoiceID)
        {

            return GetAllInvoices()
                .SingleOrDefault(i => i.Id == invoiceID);

        }

        public InvoiceWithItems UpdateInvoice(InvoiceWithItems invoiceWithItems)
        {

            // update invoice
            InvoiceRepository.Update(_mapper.Map<Invoice>(invoiceWithItems));

            // get invoice items
            var existingInvoiceItems = InvoiceItemRepository.GetByID(invoiceWithItems.Id);

            foreach (var item in existingInvoiceItems)
            {

                // check item should be deleted
                var updateItem = invoiceWithItems
                    .Items
                    .SingleOrDefault(i => i.Id == item.ItemId);

                if (updateItem is null)
                    InvoiceItemRepository.Delete(item);
                else
                {
                    InvoiceItemRepository.Update(new InvoiceItem
                    {
                        InvoiceId = invoiceWithItems.Id,
                        ItemId = updateItem.Id,
                        ItemAmount = updateItem.SoldAmount
                    });
                }

            }

            // add new items
            foreach (var item in invoiceWithItems.Items)
            {

                var shouldAdd = existingInvoiceItems.SingleOrDefault(i => i.ItemId == item.Id) is null;

                if (shouldAdd)
                    InvoiceItemRepository.Create( new InvoiceItem
                    {
                        InvoiceId = invoiceWithItems.Id, ItemId = item.Id , ItemAmount = item.SoldAmount
                    });

            }

            return GetInvoice(invoiceWithItems.Id);

        }
    }
}
