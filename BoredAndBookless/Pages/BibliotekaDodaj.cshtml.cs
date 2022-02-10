using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoredAndBookless.Models;
using MongoDB.Driver;

namespace BoredAndBookless.Pages
{
    public class BibliotekaDodajModel : PageModel
    {
        [BindProperty]
        public Biblioteka NovaBiblioteka { get; set; }

        [BindProperty]
        public int? PostojiVec { get; set; }

        [BindProperty]
        public IList<Knjiga> SveKnjige { get; set; }

        [BindProperty]
        public IList<int> IzabranaKnjiga { get; set; }

        private IMongoCollection<Biblioteka> kolekcija;

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                MongoClient client = new MongoClient("mongodb://localhost:27017");
                IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
                kolekcija = db.GetCollection<Biblioteka>("biblioteka");

                Biblioteka PostojiBiblioteka = kolekcija.Find(x => x.Naziv == NovaBiblioteka.Naziv).FirstOrDefault();

                if (PostojiBiblioteka != null)
                {
                    PostojiVec = 1;
                    return this.Page();
                }

                kolekcija.InsertOne(NovaBiblioteka);

                return RedirectToPage("./BibliotekaSve");
            }
        }
    }
}
