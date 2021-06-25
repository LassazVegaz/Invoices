using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {

        IInvoiceService invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }


        // GET: api/Invoices
        // get all invoices
        [HttpGet]
        public InvoiceWithItems[] Get()
        {
            return invoiceService.GetAllInvoices();
        }

        // GET api/Invoices/5
        // get invoice by id
        [HttpGet("{id}")]
        public ActionResult<InvoiceWithItems> Get(int id)
        {
            var invoice = invoiceService.GetInvoice(id);

            if (invoice is null)
                return NotFound($"Invoice {id} is not found");
            else
                return invoice;
        }

        // POST api/invoices
        // create invoice
        [HttpPost]
        public InvoiceWithItems Post([FromBody] InvoiceWithItems invoice)
        {

            return invoiceService.CreateInvoice(invoice);

        }

        // PUT api/invoices/5
        // update invoice
        [HttpPut("{id}")]
        public ActionResult<InvoiceWithItems> Put(int id, [FromBody] InvoiceWithItems invoice)
        {

            if (!invoiceService.InvoiceExists(id))
                return NotFound($"Invoice {id} is not found");

            return invoiceService.UpdateInvoice(invoice);

        }

        // DELETE api/invoices/5
        // delete invoice
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            if (!invoiceService.InvoiceExists(id))
                return NotFound($"Invoice {id} is not found");

            invoiceService.DeleteInvoice(id);

            return Ok($"Invoice {id} was deleted");

        }
    }
}
