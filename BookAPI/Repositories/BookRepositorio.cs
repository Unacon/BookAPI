using BookAPI.Controllers;
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
        public ConexaoController _conexao;

        public BookRepositorio(BookContext context)
        {
            _context = context;
            _conexao = new ConexaoController();
        }
        public Retorno Delete(int IdBook)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            Retorno retorno = new Retorno();

            parametros.Add("@IdBook", IdBook);
            retorno.retornoProcedure = _conexao.Executar("dbo.Bib_Livros_Del", parametros);

            if(retorno.retornoProcedure == null)
            {
                retorno.retornoProcedure = "Livro deletado com sucesso.";
            }

            return retorno;
        }

        public IEnumerable<Book> Get()
        {
            return _conexao.GetLista<Book>("dbo.Bib_Livros_Sel");
        }

        public Book Get(int IdBook)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@IdBook", IdBook);
            return _conexao.GetLista<Book>("dbo.Bib_Livros_Sel", parametros).FirstOrDefault();
        }

        public Retorno Insert(Book book)
        {
            Retorno retorno = new Retorno();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@Autor", book.Autor);
            parametros.Add("@Titulo", book.Titulo);
            parametros.Add("@Descricao", book.Descricao);

            retorno.retornoProcedure = _conexao.Executar("dbo.Bib_Livros_Ins", parametros);

            if (retorno.retornoProcedure == null)
            {
                retorno.retornoProcedure = "Livro inserido com sucesso.";
            }
            return retorno;
        }

        public Retorno Update(Book book)
        {
            Retorno retorno = new Retorno();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@IdBook", book.IdBook);
            parametros.Add("@Autor", book.Autor);
            parametros.Add("@Titulo", book.Titulo);
            parametros.Add("@Descricao", book.Descricao);

            retorno.retornoProcedure = _conexao.Executar("dbo.Bib_Livros_Upd", parametros);

            if (retorno.retornoProcedure == null)
            {
                retorno.retornoProcedure = "Livro alterado com sucesso.";
            }
            return retorno;
        }
    }
}
