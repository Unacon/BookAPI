using System.ComponentModel.DataAnnotations;

namespace BookAPI.Model
{
    public class Book
    {
        [Key]
        public int IdBook { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }
        public string Titulo { get; set; }
    }
}
