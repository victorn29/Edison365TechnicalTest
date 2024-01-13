using Edison365TechnicalTest.Data;
using Edison365TechnicalTest.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Edison365TechnicalTest.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ODataController
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 2)]
        public IQueryable<Book> GetBooks()
        {
            try
            {
                return _context.Books
                        .Include(b => b.BookAuthors)
                        .ThenInclude(ba => ba.Author);
            }
            catch
            {
                return Enumerable.Empty<Book>().AsQueryable();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(string bookName)
        {
            try
            {
                if (bookName == null)
                {
                    return BadRequest("Invalid book data");
                }

                var newBook = new Book() { Name = bookName };
                var result = _context.Books.Add(newBook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBooks), new { id = newBook.ID }, newBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
