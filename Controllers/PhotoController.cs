using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {

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
        public IActionResult Show(int id)
        {
            var pizza = PhotoManager.GetPhotoById(id);
            var categories = PhotoManager.GetAllCategories(); // Supponiamo che tu abbia un metodo per recuperare tutte le categorie
            var model = new PizzaFormModel(pizza, categories);
            model.CreateIngredients(); // Assicurati di popolare gli ingredienti
            return View(model);
        }
    }
}
