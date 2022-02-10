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
    public class RezervacijaOtkaziModel : PageModel
    {
        public ObjectId NoviId { get; set; }

        [BindProperty]
        private Rezervacija RezervacijaZaBrisanje { get; set; }

        public int Provera { get; set; }

        private IMongoCollection<Rezervacija> kolekcija;

        public IActionResult OnPost(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Rezervacija>("rezervacija");

            NoviId = ObjectId.Parse(id);
            RezervacijaZaBrisanje = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();
            kolekcija.DeleteOne(x => x.Id == RezervacijaZaBrisanje.Id);

            return RedirectToPage("./RezervacijaSve");
        }
    }
}
