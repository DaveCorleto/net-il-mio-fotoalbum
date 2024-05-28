using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System.Runtime.InteropServices;

namespace net_il_mio_fotoalbum.Data
{
    public class CategoryManager
    {
        public static int CategoryCount()
        {
            //Creo un istanza temporanea di PizzaContext
            //e con la direttiva using comunico di chiudermi la "connessione"

            using PhotoContext db = new PhotoContext();
            return db.Categories.Count();
        }
        public static List<Category> GetAllCategories()
        {
            using PhotoContext db = new PhotoContext();
            return db.Categories.ToList();
        }

        public static Category GetCategoryById(int id)
        {
            using PhotoContext photoContext = new PhotoContext();
            return photoContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public static List<Category> GetCategoryByTitle(string Title)
        {
            using PhotoContext db = new PhotoContext();
            return db.Categories.Where(x => x.Title.ToLower().Contains(Title.ToLower())).ToList();
        }


        public async Task<bool> CategoryInsert(Category newCategory)
        {
            if (newCategory == null)
            {
                throw new ArgumentNullException(nameof(newCategory));
            }

            // Se il titolo non è null nè vuoto...
            if (string.IsNullOrWhiteSpace(newCategory.Title))
            {
                throw new ArgumentException("Il titolo della categoria non può essere null o vuoto", nameof(newCategory.Title));
            }

            using PhotoContext photoContext = new PhotoContext();

            try
            {
                photoContext.Add(newCategory);
                await photoContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                // Log specifico per errori di database...è comodo 
                Console.WriteLine($"Errore di aggiornamento del database: {dbEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log generico
                Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                return false;
            }
        }



    }
}
