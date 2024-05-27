using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("Photos")]
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio: devi inserire il titolo della foto")]
        [StringLength(30, ErrorMessage = "Massimo 30 caratteri")]
        [MinLength(3)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio...descrivi la tua foto")]
        [StringLength(100, ErrorMessage = "Massimo 100 caratteri")]
        [MinLength(5)]
        public string Description { get; set; }

        // da implementare
        //[Required(ErrorMessage = "Campo obbligatorio")]
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
