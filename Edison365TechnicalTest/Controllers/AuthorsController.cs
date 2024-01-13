using Edison365TechnicalTest.Data;
using Edison365TechnicalTest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Edison365TechnicalTest.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Author> GetAuthors()
        {
            try
            {
                return _context.Authors.ToList();
            }
            catch
            {
                return Enumerable.Empty<Author>();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(string firsName, string lastName)
        {
            try
            {
                if (firsName == null || lastName == null)
                {
                    return BadRequest("Invalid author data");
                }

                var newAuthor = new Author()
                {
                    FirstName = firsName,
                    LastName = lastName
                };
                _context.Authors.Add(newAuthor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthors), new { id = newAuthor.ID }, newAuthor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
