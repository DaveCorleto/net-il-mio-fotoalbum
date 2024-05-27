using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Versioning;
using System.Reflection.Metadata;
using net_il_mio_fotoalbum.Models;
using System.Security.Policy;
using MessagePack;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace net_il_mio_fotoalbum.Data
{
    public class PhotoManager
    {
        private readonly PhotoContext _context;

        public PhotoManager(PhotoContext context)
        {
            _context = context;
        }

        //Uso dell'enum per gestire meglio nei metodi i vari tipi di risultato
        public enum ResultType
        {
            OK,
            Exception,
            NotFound
        }

        public static int PhotoCount()
        {
            //Creo un istanza temporanea di PizzaContext
            //e con la direttiva using comunico di chiudermi la "connessione"

            using PhotoContext db = new PhotoContext();
            return db.Photos.Count();
        }

        public static List<Photo> GetAllPhotos()
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.ToList();
        }


        //Recupera la foto dall'id
        public static Photo GetPhotoById(int id, bool includeReferences = true)
        {
            using PhotoContext db = new PhotoContext();
            //Se l'elemento foto che sto recuperando dal db include una category mi verrà
            //restituita anche l'informazione della category

            if (includeReferences)
                //attraverso la lambda functions includo nella richiesta della foto anche le categorie
                return db.Photos.Where(x => x.Id == id).Include(p => p.Categories).FirstOrDefault();

            //Altrimenti mi viene restituito solo l'elemento foto con valore di category null

            return db.Photos.FirstOrDefault(p => p.Id == id);
        }

        public static List<Photo> GetPhotoByTitle(string Title)
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.Where(x => x.Title.ToLower().Contains(Title.ToLower())).ToList();
        }
        public async Task<bool> PhotoInsert(Photo photo)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo));
            }

            try
            {
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                  Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                return false;
            }
        }

        public static List<Category> GetAllCategories()
        {
            using PhotoContext db = new PhotoContext();
            return db.Categories.ToList();
        }

        public static void SeedDatabase()
        {
            using PhotoContext context = new PhotoContext();

            if (PhotoCount() == 0)
            {
                byte[] LoadImage(string imagePath) => File.ReadAllBytes(imagePath);

                var photos = new List<Photo>
                {
                    new Photo
                    {
                        Title = "Norvegia in autunno",
                        Description = "Fiordo d'autunno",
                        //LoadImage(...): Questo è un metodo che si assume essere definito per caricare un'immagine da un percorso e restituirla come un array di byte (byte[]). È la funzione che converte l'immagine in dati binari che possono essere memorizzati nel database.

                        //Directory.GetCurrentDirectory(): Questo metodo restituisce il percorso della directory corrente in cui l'applicazione è in esecuzione. In un'applicazione web .NET, questo sarà il percorso della cartella radice del progetto.

                        Image = LoadImage(Directory.GetCurrentDirectory() + "/wwwroot/img/1.jpeg"),
                        IsVisible = true,
                        Categories = new List<Category>()
                    },
                    new Photo
                    {
                        Title = "Seconda Foto",
                        Description = "Descrizione della seconda foto",
                        Image = LoadImage(Directory.GetCurrentDirectory() + "/wwwroot/img/2.jpg"),
                        IsVisible = true,
                        Categories = new List<Category>()
                    }
                    // Aggiungi altre foto se necessario
                };

                foreach (var photo in photos)
                {
                    context.Photos.Add(photo);
                }

                context.SaveChanges();
            }
        }
    }
}

