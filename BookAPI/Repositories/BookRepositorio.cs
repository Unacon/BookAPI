using BookAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class BookRepositorio : IBookRepositorio
    {
        public BookContext _context; 

        public BookRepositorio(BookContext context)
        {
            _context = context; 
        }
        public async Task Delete(int Idbook)
        {
            Book bookToDelete = await _context.Books.FindAsync(Idbook);
            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> Get(int IdBook)
        {
            return await _context.Books.FindAsync(IdBook);
        }

        public async Task<Book> Insert(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task Update(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
