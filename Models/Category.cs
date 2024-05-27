using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Photo> Photos { get; set; }

        public Category()
        {
            Photos = new List<Photo>(); // Inizializza Photos a una lista vuota
        }

        public Category(int id, string title, List<Photo> photos)
        {
            Id = id;
            Title = title;
            //Se photos è null, usa una nuova lista vuota di Photo; altrimenti, usa photos".
            Photos = photos ?? new List<Photo>(); 
        }
    }
}



