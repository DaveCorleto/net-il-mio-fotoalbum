﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Versioning;
using System.Reflection.Metadata;
using net_il_mio_fotoalbum.Models;
using System.Security.Policy;
using MessagePack;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        public async Task<bool> PhotoInsert(Photo photo, List<string> selectedCategoryIds)
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

        public static bool PhotoEdit(int id, Photo photo, List<string> selectedCategoryIds)
        {
            try
            {
                using PhotoContext db = new PhotoContext();
                var photoMod = db.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

                if (photoMod == null)
                {
                    return false;
                }

                // Aggiorna le proprietà della foto
                photoMod.Title = photo.Title;
                photoMod.Description = photo.Description;
                photoMod.Image = photo.Image;
                photoMod.IsVisible = photo.IsVisible;

                // Svuota le categorie attuali
                photoMod.Categories.Clear();

                // Aggiungi le categorie selezionate
                if (selectedCategoryIds != null)
                {
                    foreach (var categoryIdString in selectedCategoryIds)
                    {
                        if (int.TryParse(categoryIdString, out int categoryId))
                        {
                            var categoryFromDb = db.Categories.FirstOrDefault(c => c.Id == categoryId);
                            if (categoryFromDb != null)
                            {
                                photoMod.Categories.Add(categoryFromDb);
                            }
                        }
                        else
                        {
                            // Log per i casi in cui il parsing fallisce
                            Console.WriteLine($"Failed to parse category ID: {categoryIdString}");
                        }
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log dell'eccezione per il debug
                Console.WriteLine($"An error occurred while editing the photo: {ex.Message}");
                return false;
            }
        }



        public static bool PhotoDelete(int id)
        {
            try
            {
                //Qui recupero la foto selezionata con il metodo Rec...NON creo un nuovo context del db perchè sta già lavorando quello
                //che utilizza GetPhotoById...

                var photo = GetPhotoById(id, false); 
                if (photo == null)
                    return false;

                //qui invece DEVO creare un nuovo contesto per essere sicuro che la rimozione dell'oggetto pizza venga tracciato dal DB'

                using PhotoContext db = new PhotoContext();
                db.Remove(photo);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                Image = LoadImage(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "1.jpeg")),
                IsVisible = true,
                Categories = new List<Category>()
            },
            new Photo
            {
                Title = "Seconda Foto",
                Description = "Descrizione della seconda foto",
                Image = LoadImage(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "2.jpg")),
                IsVisible = true,
                Categories = new List<Category>()
            }
            // Aggiungi altre foto se necessario
        };

                context.Photos.AddRange(photos);
                context.SaveChanges();
            }
        }


        internal static void PhotoInsert(Photo photo, List<SelectListItem>? categories)
        {
            throw new NotImplementedException();
        }
    }
}

