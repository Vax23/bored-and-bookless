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
    public class KnjigaObrisiModel : PageModel
    {
        public ObjectId NoviID { get; set; }

        [BindProperty]
        private Knjiga KnjigaBrisanje { get; set; }

        public int Provera { get; set; }

        private IMongoCollection<Knjiga> kolekcija;

        public IActionResult OnPost(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            NoviID = ObjectId.Parse(id);
            KnjigaBrisanje = kolekcija.Find(x => x.Id == NoviID).FirstOrDefault();

            if (KnjigaBrisanje.Rezervacije.Count == 0)
            {
                kolekcija.DeleteOne(x => x.Id == KnjigaBrisanje.Id);
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
