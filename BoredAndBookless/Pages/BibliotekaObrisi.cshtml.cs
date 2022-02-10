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
    public class BibliotekaObrisiModel : PageModel
    {
        public ObjectId NoviId { get; set; }

        [BindProperty]
        private Biblioteka BibliotekaBrisanje { get; set; }

        public int Provera { get; set; }

        private IMongoCollection<Biblioteka> kolekcija;

        public IActionResult OnPost(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Biblioteka>("biblioteka");

            NoviId = ObjectId.Parse(id);
            BibliotekaBrisanje = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();

            if (BibliotekaBrisanje.Knjige.Count == 0)
            {
                kolekcija.DeleteOne(x => x.Id == BibliotekaBrisanje.Id);
                return RedirectToPage("./BibliotekaSve");
            }
            else
            {
                Provera = 1;
                return Page();
            }
        }
    }
}
