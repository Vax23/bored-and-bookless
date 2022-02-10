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
    public class KnjigaSveModel : PageModel
    {
        [BindProperty]
        public IList<Knjiga> SveKnjige { get; set; }

        private IMongoCollection<Knjiga> kolekcija;
        public ObjectId BibliotekaId { get; set; }

        public SelectList SviZanrovi { get; set; }

        [BindProperty]
        public IList<Knjiga> KnjigaFiltrirano { get; set; }

        [BindProperty]
        public string IzabraniZanr { get; set; }

        public void OnGet(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            BibliotekaId = ObjectId.Parse(id);
            SveKnjige = kolekcija.Find(x => x.Biblioteka.Id == BibliotekaId).ToList();

            IList<string> Zanrovi = new List<string>();
            IList<string> z = new List<string>();

            for (var i = 0; i < SveKnjige.Count(); i++)
            {
                if (Zanrovi.Count != 0)
                {
                    for (int j = 0; j < Zanrovi.Count(); j++)
                    {
                        if (Zanrovi[j] != SveKnjige[i].Zanr)
                        {
                            Zanrovi.Add(SveKnjige[i].Zanr);
                        }
                    }
                }
                else
                {
                    Zanrovi.Add(SveKnjige[i].Zanr);
                }
            }
            SviZanrovi = new SelectList(Zanrovi.Distinct());
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Knjiga>("knjiga");

            BibliotekaId = ObjectId.Parse(id);
            SveKnjige = kolekcija.Find(x => x.Biblioteka.Id == BibliotekaId).ToList();
            IList<string> Zanrovi = new List<string>();

            for (var i = 0; i < SveKnjige.Count(); i++)
            {
                if (Zanrovi.Count != 0)
                {
                    for (int j = 0; j < Zanrovi.Count(); j++)
                    {
                        if (Zanrovi[j] != SveKnjige[i].Zanr)
                        {
                            Zanrovi.Add(SveKnjige[i].Zanr);
                        }
                    }
                }
                else
                {
                    Zanrovi.Add(SveKnjige[i].Zanr);
                }
            }

            SviZanrovi = new SelectList(Zanrovi.Distinct());

            if (IzabraniZanr != "Prikaži sve")
            {
                SveKnjige = kolekcija.Find(x => x.Zanr == IzabraniZanr).ToList();
            }
            return Page();
        }
    }
}
