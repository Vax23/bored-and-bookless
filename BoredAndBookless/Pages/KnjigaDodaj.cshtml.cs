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
    public class KnjigaDodajModel : PageModel
    {
        [BindProperty]
        public Knjiga NovaKnjiga { get; set; }

        public ObjectId NoviId;

        [BindProperty]
        public int? PostojiVec { get; set; }

        private IMongoCollection<Knjiga> kolekcija;

        [BindProperty]
        public Biblioteka PostojiBiblioteka { get; set; }

        private IMongoCollection<Biblioteka> kolekcijaB;

        public IActionResult OnPost(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                MongoClient client = new MongoClient("mongodb://localhost:27017");
                IMongoDatabase db = client.GetDatabase("BoredAndBooklessDB");
                kolekcija = db.GetCollection<Knjiga>("knjiga");

                NoviId = ObjectId.Parse(id);
                Knjiga PostojiKnjiga = kolekcija.Find(x => x.Naziv == NovaKnjiga.Naziv && x.Biblioteka.Id == NoviId).FirstOrDefault();
                kolekcijaB = db.GetCollection<Biblioteka>("biblioteka");
                PostojiBiblioteka = kolekcijaB.Find(x => x.Id == NoviId).FirstOrDefault();

                if (PostojiKnjiga != null)
                {
                    PostojiVec = 1;
                    return this.Page();
                }

                kolekcija.InsertOne(NovaKnjiga);

                PostojiBiblioteka.Knjige.Add(new MongoDBRef("knjiga", NovaKnjiga.Id));
                NovaKnjiga.Biblioteka = new MongoDBRef("biblioteka", NoviId);

                kolekcija.ReplaceOne(x => x.Id == NovaKnjiga.Id, NovaKnjiga);
                kolekcijaB.ReplaceOne(x => x.Id == PostojiBiblioteka.Id, PostojiBiblioteka);

                return RedirectToPage("./KnjigaSve", new { id = id });
            }
        }
    }
}
