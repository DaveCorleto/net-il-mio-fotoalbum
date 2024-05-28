using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System.Runtime.InteropServices;

namespace net_il_mio_fotoalbum.Data
{
    public class CategoryManager
    {
        public static int CategoryCount()
        {
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

        public static List<Category> GetCategoryByTitle(string title)
        {
            using PhotoContext db = new PhotoContext();
            return db.Categories.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public async Task<bool> CategoryInsert(Category newCategory)
        {
            if (newCategory == null)
            {
                throw new ArgumentNullException(nameof(newCategory));
            }

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
                Console.WriteLine($"Errore di aggiornamento del database: {dbEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                return false;
            }
        }

        public static bool CategoryDelete(int id)
        {
            try
            {
                var cat = GetCategoryById(id);
                if (cat == null)
                    return false;

                using PhotoContext db = new PhotoContext();
                db.Remove(cat);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CategoryUpdate(Category updatedCategory)
        {
            if (updatedCategory == null)
            {
                throw new ArgumentNullException(nameof(updatedCategory));
            }

            using PhotoContext photoContext = new PhotoContext();

            try
            {
                var existingCategory = photoContext.Categories.Find(updatedCategory.Id);
                if (existingCategory == null)
                {
                    return false;
                }

                existingCategory.Title = updatedCategory.Title;

                await photoContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Errore di aggiornamento del database: {dbEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                return false;
            }
        }

    }

}
