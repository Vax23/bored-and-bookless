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
    public class RezervacijaDodajModel : PageModel
    {
        public ObjectId KnjigaId { get; set; }

        [BindProperty]
        public Knjiga OvaKnjiga { get; set; }

        [BindProperty]
        public Korisnik TrenutniKorisnik { get; set; }

        [BindProperty]
        public Rezervacija RezervacijaKnjige { get; set; }

        [BindProperty]
        public string DanasnjiDatum { get; set; }

        [BindProperty]
        public int NeuspesnaRezervacija { get; set; }

        private IMongoCollection<Knjiga> kolekcija;
        private IMongoCollection<Rezervacija> kolekcijaR;
        private IMongoCollection<Korisnik> kolekcijaK;

        public IActionResult OnGet(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            KnjigaId = ObjectId.Parse(id);
            OvaKnjiga = kolekcija.Find(x => x.Id == KnjigaId).FirstOrDefault();

            String month = DateTime.Now.Month.ToString();
            if (DateTime.Now.Month < 10)
            {
                month = month.Insert(0, "0");
            }
            String day = DateTime.Now.Day.ToString();
            if (DateTime.Now.Day < 10)
            {
                day = day.Insert(0, "0");
            }
            DanasnjiDatum = DateTime.Now.Year.ToString() + "-" + month + "-" + day;

            return this.Page();
        }

        public IActionResult OnPost(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");
            KnjigaId = ObjectId.Parse(id);
            OvaKnjiga = kolekcija.Find(x => x.Id == KnjigaId).FirstOrDefault();
            kolekcijaR = db.GetCollection<Rezervacija>("rezervacija");
            kolekcijaK = db.GetCollection<Korisnik>("korisnik");

            if (SessionClass.SessionId != null)
            {
                ObjectId KorisnikId = ObjectId.Parse(SessionClass.SessionId);
                TrenutniKorisnik = kolekcijaK.Find(x => x.Id == KorisnikId).FirstOrDefault();
            }

            if (RezervacijaKnjige.DatumUzimanja > RezervacijaKnjige.DatumVracanja)
            {
                NeuspesnaRezervacija = 1;
                return this.Page();
            }

            kolekcijaR.InsertOne(RezervacijaKnjige);
            OvaKnjiga.Rezervacije.Add(new MongoDBRef("rezervacija", RezervacijaKnjige.Id));
            TrenutniKorisnik.Rezervacije.Add(new MongoDBRef("rezervacija", RezervacijaKnjige.Id));
            RezervacijaKnjige.Knjiga = new MongoDBRef("knjiga", KnjigaId);
            RezervacijaKnjige.Korisnik = new MongoDBRef("korisnik", TrenutniKorisnik.Id);

            kolekcijaR.ReplaceOne(x => x.Id == RezervacijaKnjige.Id, RezervacijaKnjige);
            kolekcija.ReplaceOne(x => x.Id == OvaKnjiga.Id, OvaKnjiga);
            kolekcijaK.ReplaceOne(x => x.Id == TrenutniKorisnik.Id, TrenutniKorisnik);
            return RedirectToPage("./RezervacijaSve");

            return Page();
        }
    }
}
