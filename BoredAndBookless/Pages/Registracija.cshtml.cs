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
    public class RegistracijaModel : PageModel
    {
        [BindProperty]
        public Korisnik NoviKorisnik { get; set; }

        [BindProperty]
        public bool PostojiVec { get; set; }

        private IMongoCollection<Korisnik> kolekcija;

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
                kolekcija = db.GetCollection<Korisnik>("korisnik");

                Korisnik PostojiUsername = kolekcija.Find(x => x.Username == NoviKorisnik.Username).FirstOrDefault();

                if (PostojiUsername != null)
                {
                    PostojiVec = true;
                    return this.Page();
                }

                NoviKorisnik.Tip = "K";
                kolekcija.InsertOne(NoviKorisnik);

                return RedirectToPage("./Prijava");
            }
        }
    }
}
