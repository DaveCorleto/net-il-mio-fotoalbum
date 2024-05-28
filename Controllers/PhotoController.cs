using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

                var categories = CategoryManager.GetAllCategories();
                var model = new PhotoFormModel(photo, categories);

                return View(model);
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
        public IActionResult CreatePhoto(PhotoFormModel photoDaInserire)
        {
            //Se i dati NON sono validi:

            if (ModelState.IsValid == false)
            {
                // Ritorno la form di prima con i dati della foto
                // precompilati dall'utente

                return View("Create", photoDaInserire);
            }
            //Altrimenti se SONO validi inserisco a db la nuova pizza

            PhotoManager.PhotoInsert(photoDaInserire.Photo, photoDaInserire.Categories);

            // Richiamo la action Index affinché vengano mostrate tutte le pizze
            // inclusa quella nuova
            return RedirectToAction("Index");

        }

        public IActionResult UpdatePhoto(int id) // Restituisce la form per l'update di una foto
        {
            //Recupero la pizza (id) e la assegno a var pizza 
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
                    Text = c.Title // o un'altra proprietà che desideri visualizzare
                }).ToList();

                photoDaModificare.Categories = categories;
                photoDaModificare.CreateCategory();
                return View("UpdatePizza", photoDaModificare);
            }

            var photoMod = PhotoManager.PhotoEdit(id, photoDaModificare.Photo, photoDaModificare.SelectedCategoryIds);
            if (photoMod)
            {
                // Richiamiamo la action Index affinché vengano mostrate tutte le foto
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }



    }
}
