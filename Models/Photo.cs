using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Models
{

    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; } // Per memorizzare i dati binari dell'immagine
        public bool IsVisible { get; set; }

        // Relazione molti a molti con Category
        public List<Category> Categories { get; set; }

        // Costruttore senza parametri
        public Photo()
        {
            Categories = new List<Category>();
            IsVisible = true; // Imposta IsVisible a true per impostazione predefinita
        }

        // Costruttore con parametri, con valore predefinito per isVisible
        public Photo(int id, string title, string description, byte[] image, List<Category> categories, bool isVisible = true)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            IsVisible = isVisible; // Assegna il valore passato o usa il valore predefinito true
            Categories = categories ?? new List<Category>(); // Gestisci il caso in cui categories sia null
        }
    }

}
