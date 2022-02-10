using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BoredAndBookless.Models
{
    public class Knjiga
    {
        public ObjectId Id { get; set; }

        public string Naziv { get; set; }

        public string Autor { get; set; }

        public string Zanr { get; set; }

        public string Opis { get; set; }

        public string Slika { get; set; }

        public MongoDBRef Biblioteka { get; set; }

        public List<MongoDBRef> Rezervacije { get; set; }

        public Knjiga()
        {
            Rezervacije = new List<MongoDBRef>();
        }
    }
}
