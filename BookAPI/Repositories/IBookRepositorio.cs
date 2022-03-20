using BookAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public interface IBookRepositorio
    {
        IEnumerable<Book> Get();
        Book Get(int IdBook);
        Retorno Insert(Book book);
        Retorno Update(Book book);
        Retorno Delete(int Idbook);
    }
}
