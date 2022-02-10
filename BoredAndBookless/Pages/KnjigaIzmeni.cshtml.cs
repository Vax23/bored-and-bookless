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
    public class KnjigaIzmaniModel : PageModel
    {
        [BindProperty]
        public Knjiga TrenutnaKnjiga { get; set; }

        [BindProperty]
        public Knjiga NovaKnjiga { get; set; }

        public ObjectId NoviId;

        private IMongoCollection<Knjiga> kolekcija;

        public IActionResult OnGet(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            NoviId = ObjectId.Parse(id);
            TrenutnaKnjiga = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();

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
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            NoviId = ObjectId.Parse(id);
            NovaKnjiga = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();
            NovaKnjiga.Naziv = TrenutnaKnjiga.Naziv;
            NovaKnjiga.Autor = TrenutnaKnjiga.Autor;
            NovaKnjiga.Zanr = TrenutnaKnjiga.Zanr;
            NovaKnjiga.Opis = TrenutnaKnjiga.Opis;
            kolekcija.ReplaceOne(x => x.Id == NovaKnjiga.Id, NovaKnjiga);

            return RedirectToPage("./KnjigaJedna", new { id = id });
        }
    }
}
