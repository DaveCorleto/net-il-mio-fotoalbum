using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; } // Oggetto Photo che l'utente sta creando o modificando
        public List<Category> categories { get; set; }
        public List<SelectListItem>? Categories { get; set; } // Lista delle categorie disponibili e selezionabili per un dropdown o selezione
        public List<string>? SelectedCategoryIds { get; set; } // Lista degli ID delle categorie selezionate dall'utente

        public PhotoFormModel()
        {
            Categories = new List<SelectListItem>();
            SelectedCategoryIds = new List<string>();
        }

        // Costruttore della form
        public PhotoFormModel(Photo p, List<Category> c)
        {
            this.Photo = p;
            this.categories = c;
            // Definisco gli ingredienti selezionati tramite una lista.
            SelectedCategoryIds = new List<string>();

            // Se la lista degli ingredienti non è vuota
            if (Photo.Categories != null)
            {
                // Aggiungo gli ingredienti selezionati agli ingredienti della pizza
                foreach (var i in Photo.Categories)
                {
                    SelectedCategoryIds.Add(i.Id.ToString());
                }
            }
        }

        public void CreateCategory()
        {
            // Inizializza la lista di SelectListItem per le categorie
            this.Categories = new List<SelectListItem>();

            // Se la lista degli ingredienti selezionati è null, la inizializza come una nuova lista vuota
            if (this.SelectedCategoryIds == null)
            {
                this.SelectedCategoryIds = new List<string>();
            }

            // Ottengo tutti gli ingredienti disponibili dal database
            var CategoriesFromDB = Data.CategoryManager.GetAllCategories();

            // Itero attraverso ogni categoria
            foreach (var cat in CategoriesFromDB)
            {
                // Verifico se la categoria corrente è nella lista delle categorie selezionate
                bool isSelected = this.SelectedCategoryIds.Contains(cat.Id.ToString());

                // Aggiunge un nuovo SelectListItem alla lista degli ingredienti
                this.Categories.Add(new SelectListItem
                {
                    Text = cat.Title,
                    Value = cat.Id.ToString(), // SelectListItem vuole una generica stringa, non un int
                    Selected = isSelected
                });
            }
        }
    }
}

