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
    public class ProfilIzmeniModel : PageModel
    {
        [BindProperty]
        public Korisnik TrenutniKorisnik { get; set; }

        [BindProperty]
        public Korisnik NoviKorisnik { get; set; }

        public ObjectId NoviId;

        private IMongoCollection<Korisnik> kolekcija;

        public IActionResult OnGet(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Korisnik>("korisnik");
            NoviId = ObjectId.Parse(id);
            TrenutniKorisnik = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();
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
            kolekcija = db.GetCollection<Korisnik>("korisnik");

            NoviId = ObjectId.Parse(id);
            NoviKorisnik = kolekcija.Find(x => x.Id == NoviId).FirstOrDefault();
            NoviKorisnik.Ime = TrenutniKorisnik.Ime;
            NoviKorisnik.Prezime = TrenutniKorisnik.Prezime;
            NoviKorisnik.Email = TrenutniKorisnik.Email;
            NoviKorisnik.Broj = TrenutniKorisnik.Broj;
            NoviKorisnik.Username = TrenutniKorisnik.Username;
            NoviKorisnik.Password = TrenutniKorisnik.Password;
            kolekcija.ReplaceOne(x => x.Id == NoviKorisnik.Id, NoviKorisnik);
            return RedirectToPage("./Profil", new { id = id });
        }
    }

   
}
