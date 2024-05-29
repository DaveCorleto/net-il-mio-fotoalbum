using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        //Logger per la gestione errori 

        private readonly ILogger<PhotoController> _logger;
        public PhotoController(ILogger<PhotoController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public IActionResult Index()
        {
            return View(PhotoManager.GetAllPhotos());
        }

        public IActionResult AdminPage()
        {
            PhotoContext photoContext = new PhotoContext();
            var photos = photoContext.Photos.Include(p => p.Categories).ToList();
            return View(photos);
        }



        [HttpGet]
        public IActionResult GetPhotos(int id)
        {
            try
            {
                var photo = PhotoManager.GetPhotoById(id);
                if (photo != null)
                    return View(photo);
                else
                    //return NotFound();
                    return View("Errore", new ErrorViewModel($"La foto {id} non è stata trovata!"));
            }
            catch (Exception e)
            {
                return View("Errore", new ErrorViewModel(e.Message));
                //return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public IActionResult ShowPhoto(int id)
        {
            try
            {
                var photo = PhotoManager.GetPhotoById(id);
                if (photo == null)
                {
                    return NotFound();
                }

                return View(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        public IActionResult CreatePhoto()
        {
            // Recupera tutte le categorie disponibili
            List<Category> categories = CategoryManager.GetAllCategories();

            // Crea un nuovo oggetto PhotoFormModel
            PhotoFormModel model = new PhotoFormModel
            {
                Photo = new Photo
                {
                    Title = "Inserisci il titolo della foto",
                    Description = "Inserisci Descrizione",
                    Categories = new List<Category>(),
                    IsVisible = true
                },
                categories = categories
            };

            // Passa il modello alla vista
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhoto(PhotoFormModel photoDaInserire)
        {
            if (ModelState.IsValid == false)
            {
                photoDaInserire.categories = CategoryManager.GetAllCategories();
                return View("CreatePhoto", photoDaInserire);
            }

            try
            {
                // Creare un oggetto Photo dal modello ricevuto
                var photo = new Photo
                {
                    Title = photoDaInserire.Photo.Title,
                    Description = photoDaInserire.Photo.Description,
                    Image = photoDaInserire.Photo.Image,
                    IsVisible = photoDaInserire.Photo.IsVisible,
                    Categories = photoDaInserire.Photo.Categories
                };


                var photoManager = new PhotoManager();

                if (!await photoManager.PhotoInsert(photo, photoDaInserire))
                {
                    return View("Error");
                }
                else
                {
                    return RedirectToAction("AdminPage", "Photo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'inserimento della foto: {ex.Message}");
                return View("Error");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreatePhoto(PhotoFormModel photoDaInserire, IFormFile image)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        photoDaInserire.categories = CategoryManager.GetAllCategories();
        //        return View("CreatePhoto", photoDaInserire);
        //    }

        //    try
        //    {
        //        // Creare un oggetto Photo dal modello ricevuto
        //        var photo = new Photo
        //        {
        //            Title = photoDaInserire.Photo.Title,
        //            Description = photoDaInserire.Photo.Description,
        //            IsVisible = photoDaInserire.Photo.IsVisible
        //        };

        //        // Leggi il file caricato dall'utente e convertilo in un array di byte
        //        if (image != null)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await image.CopyToAsync(memoryStream);
        //                photo.Image = memoryStream.ToArray();
        //            }
        //        }

        //        // Continua con il salvataggio nel database
        //        // ...
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Errore durante l'inserimento della foto: {ex.Message}");
        //        return View("Error");
        //    }
        //}




        public IActionResult UpdatePhoto(int id) // Restituisce la form per l'update di una foto
        {

            var photo = PhotoManager.GetPhotoById(id);

            if (photo == null)
                return NotFound();
            //Popolo la form con i dati
            PhotoFormModel model = new PhotoFormModel(photo, CategoryManager.GetAllCategories());

            model.CreateCategory();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePhoto(int id, PhotoFormModel photoDaModificare)
        {
            if (ModelState.IsValid == false)
            {
                // Converto in una lista di SelectListItem
                var categories = Data.CategoryManager.GetAllCategories().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                }).ToList();

                photoDaModificare.Categories = categories;
                photoDaModificare.CreateCategory();
                return View("UpdatePhoto", photoDaModificare);
            }

            var photoMod = PhotoManager.PhotoEdit(id, photoDaModificare.Photo, selectedCategoryIds: photoDaModificare.SelectedCategoryIds);
            if (photoMod)
            {
               
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhoto(int id)
        {
            try
            {
                var photoToDelete = PhotoManager.GetPhotoById(id);

                if (photoToDelete == null)
                {
                    return NotFound();
                }

                bool deleteSuccess = PhotoManager.PhotoDelete(id);

                if (deleteSuccess)
                {
                    return RedirectToAction("AdminPage");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'eliminazione della foto: {ex.Message}");
                return View("Error");
            }
        }


    }
}
