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
        public IEnumerable<Book> GetBook()
        {
            return _bookRepositorio.Get();
        }

        [HttpGet("{IdBook}")]
        public ActionResult<Book> GetBook(int IdBook)
        {
            return _bookRepositorio.Get(IdBook);
        }

        [HttpPost]
        public ActionResult<Retorno> PostBook([FromBody] Book book)
        {
            return _bookRepositorio.Insert(book);
        }

        [HttpDelete("{Idbook}")]
        public ActionResult<Retorno> DeleteBook(int Idbook)
        {
            return _bookRepositorio.Delete(Idbook);
        }

        [HttpPut]
        public ActionResult<Retorno> Update([FromBody] Book book)
        {
            return _bookRepositorio.Update(book);
        }
    }
}
