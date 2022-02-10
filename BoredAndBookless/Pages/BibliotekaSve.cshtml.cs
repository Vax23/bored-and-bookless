using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoredAndBookless.Models;
using MongoDB.Driver;


namespace BoredAndBookless.Pages
{
    public class BibliotekaSveModel : PageModel
    {
        [BindProperty]
        public IList<Biblioteka> SveBiblioteke { get; set; }

        public SelectList SviGradovi { get; set; }

        [BindProperty]
        public IList<Biblioteka> BibliotekaFiltrirano { get; set; }

        [BindProperty]
        public string IzabraniGrad { get; set; }

        public IMongoCollection<Biblioteka> kolekcija;

        public void OnGet()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Biblioteka>("biblioteka");

            SveBiblioteke = kolekcija.Find(x => true).ToList();

            IList<string> Gradovi = new List<string>();
            IList<string> g = new List<string>();

            for (var i = 0; i < SveBiblioteke.Count(); i++)
            {
                if (Gradovi.Count != 0)
                {
                    for (int j = 0; j < Gradovi.Count(); j++)
                    {
                        if (Gradovi[j] != SveBiblioteke[i].Grad)
                        {
                            Gradovi.Add(SveBiblioteke[i].Grad);
                        }
                    }
                }
                else
                {
                    Gradovi.Add(SveBiblioteke[i].Grad);
                }
            }
            SviGradovi = new SelectList(Gradovi.Distinct());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
            kolekcija = db.GetCollection<Biblioteka>("biblioteka");

            SveBiblioteke = await kolekcija.Find(x => true).ToListAsync();
            IList<string> Gradovi = new List<string>();

            for (var i = 0; i < SveBiblioteke.Count(); i++)
            {
                if (Gradovi.Count != 0)
                {
                    for (int j = 0; j < Gradovi.Count(); j++)
                    {
                        if (Gradovi[j] != SveBiblioteke[i].Grad)
                        {
                            Gradovi.Add(SveBiblioteke[i].Grad);
                        }
                    }
                }
                else
                {
                    Gradovi.Add(SveBiblioteke[i].Grad);
                }
            }

            SviGradovi = new SelectList(Gradovi.Distinct());
            if (IzabraniGrad != "Prikaži sve")
            {
                SveBiblioteke = kolekcija.Find(x => x.Grad == IzabraniGrad).ToList();
            }
            return Page();
        }
    }
}
