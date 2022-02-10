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
    public class RezervacijaSveModel : PageModel
    {
        [BindProperty]
        public IList<Rezervacija> SveRezervacije { get; set; }

        [BindProperty]
        public IList<Knjiga> SveKnjige { get; set; }

        [BindProperty]
        public IList<Korisnik> SviKorisnici { get; set; }

        [BindProperty]
        public IList<Biblioteka> SveBiblioteke { get; set; }

        [BindProperty]
        public IList<Rezervacija> MojeRezervacije { get; set; }

        private IMongoCollection<Rezervacija> kolekcija;

        private IMongoCollection<Knjiga> kolekcijaK;

        private IMongoCollection<Korisnik> kolekcijaKo;

        private IMongoCollection<Biblioteka> kolekcijaB;

        public void OnGet()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Rezervacija>("rezervacija");
            kolekcijaK = db.GetCollection<Knjiga>("knjiga");
            kolekcijaB = db.GetCollection<Biblioteka>("biblioteka");
            kolekcijaKo = db.GetCollection<Korisnik>("korisnik");

            SveKnjige = kolekcijaK.Find(x => true).ToList();
            SveRezervacije = kolekcija.Find(x => true).ToList();
            SveBiblioteke = kolekcijaB.Find(x => true).ToList();
            SviKorisnici = kolekcijaKo.Find(x => true).ToList();

            if (SessionClass.SessionId != null)
            {
                ObjectId KorisnikId = ObjectId.Parse(SessionClass.SessionId);
                MojeRezervacije = kolekcija.Find(x => x.Korisnik.Id == KorisnikId).ToList();
            }
        }
    }
}
