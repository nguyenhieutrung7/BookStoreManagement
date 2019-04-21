using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BookStock>> Get()
        {
            List<BookStock> listBookStock = new List<BookStock>();
            BookStock bookStock = new BookStock() { Title = "Phi Lý Trí", Description = "Best Book", AuthorName = "Don Ariel", CategoryName = "Kinh Tế", ISBN = "4235435435", PublisherName = "NXB Trẻ", Price = 20000, Quantity = 15 };
            listBookStock.Add(bookStock);
            return listBookStock;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
