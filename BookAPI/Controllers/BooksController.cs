using BookAPI.Model;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepositorio _bookRepositorio;
        public BooksController(IBookRepositorio bookRepositorio)
        {
            _bookRepositorio = bookRepositorio;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBook()
        {
            return await _bookRepositorio.Get();
        }

        [HttpGet("{IdBook}")]
        public async Task<ActionResult<Book>> GetBook(int IdBook)
        {
            return await _bookRepositorio.Get(IdBook);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            Book validaBook = await _bookRepositorio.Get(book.IdBook);
            if(validaBook == null)
            {
                Book newBook = await _bookRepositorio.Insert(book);
                return CreatedAtAction(nameof(GetBook), new { id = newBook.IdBook }, newBook);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{Idbook}")]
        public async Task<ActionResult> DeleteBook(int Idbook)
        {
            Book bookToDelete = await _bookRepositorio.Get(Idbook);
            if (bookToDelete != null)
            {
                await _bookRepositorio.Delete(bookToDelete.IdBook);
                return NoContent();
            }
            else
            {
            return NotFound();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int idBook, [FromBody] Book book)
        {
            if (book.IdBook == idBook)
            {
                Book validaBook = await _bookRepositorio.Get(idBook);
                if(validaBook == null)
                {
                    return BadRequest();
                }
                await _bookRepositorio.Update(book);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
