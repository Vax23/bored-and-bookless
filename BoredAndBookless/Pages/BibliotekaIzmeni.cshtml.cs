using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoredAndBookless.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BoredAndBookless.Pages
{
    public class BibliotekaIzmeniModel : PageModel
    {
        [BindProperty]
        public Biblioteka TrenutnaBiblioteka { get; set; }

        [BindProperty]
        public Biblioteka NovaBiblioteka { get; set; }

        public ObjectId NoviId;

        private IMongoCollection<Biblioteka> kolekcija;

        public IActionResult OnGet(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Biblioteka>("biblioteka");

            NoviId = ObjectId.Parse(id);
            TrenutnaBiblioteka = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Biblioteka>("biblioteka");

            NoviId = ObjectId.Parse(id);
            NovaBiblioteka = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();
            NovaBiblioteka.Naziv = TrenutnaBiblioteka.Naziv;
            NovaBiblioteka.Grad = TrenutnaBiblioteka.Grad;
            NovaBiblioteka.Opis = TrenutnaBiblioteka.Opis;
            kolekcija.ReplaceOne(x => x.Id == NovaBiblioteka.Id, NovaBiblioteka);

            return RedirectToPage("./BibliotekaJedna", new { id = id });
        }
    }
}
