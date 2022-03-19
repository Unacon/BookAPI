using BookAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public interface IBookRepositorio
    {
        Task<IEnumerable<Book>> Get();
        Task<Book> Get(int IdBook);
        Task<Book> Insert(Book book);
        Task Update(Book book);
        Task Delete(int Idbook);
    }
}
