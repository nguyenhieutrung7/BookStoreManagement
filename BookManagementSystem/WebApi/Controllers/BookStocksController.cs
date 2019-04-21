using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("bookstocks")]
    [ApiController]
    public class BookStocksController : ControllerBase
    {
        private readonly TodoContext _context;

        public BookStocksController(TodoContext context)
        {
            _context = context;

            if (_context.BookStocks.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                BookStock bookStock = new BookStock() { Title = "Phi Lý Trí", Description = "Best Book", AuthorName = "Don Ariel", CategoryName = "Kinh Tế", ISBN = "4235435435", PublisherName = "NXB Trẻ", Price = 20000, Quantity = 15 };
                _context.BookStocks.Add(bookStock);
                _context.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookStock>>> Get()
        {
            return await _context.BookStocks.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("/bookstocks/{id}")]
        public async Task<ActionResult<BookStock>> Get(long id)
        {
            var bookStock = _context.BookStocks.FindAsync(id);
            if (bookStock == null)
            {
                return NotFound();
            }
            return await bookStock;
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<BookStock> Post([FromBody]BookStock bookStock)
        {
            _context.Add(bookStock);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = bookStock.Id }, bookStock);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult<BookStock> Put(long id, [FromBody]BookStock bookStock)
        {
            if (id != bookStock.Id)
            {
                return BadRequest();
            }
            _context.Entry(bookStock).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            var bookStock = _context.BookStocks.Find(id);
            if (bookStock == null)
            {
                return NotFound();
            }
            _context.BookStocks.Remove(bookStock);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
