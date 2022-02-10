using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoredAndBookless.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BoredAndBookless.Pages
{
    public class ProfilModel : PageModel
    {
        [BindProperty]
        public Korisnik TrenutniKorisnik { get; set; }


        private IMongoCollection<Korisnik> kolekcija;

        public void OnGet()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Korisnik>("korisnik");
           
            if (SessionClass.SessionId != null)
            {
                ObjectId KorisnikId = ObjectId.Parse(SessionClass.SessionId);
                TrenutniKorisnik = kolekcija.Find(x => x.Id == KorisnikId).FirstOrDefault();
            }

            
        }
    }
}
