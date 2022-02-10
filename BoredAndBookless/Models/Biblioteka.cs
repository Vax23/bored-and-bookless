using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BoredAndBookless.Models
{
    public class Biblioteka
    {
        public ObjectId Id { get; set; }
        public string Naziv { get; set; }

        public string Grad { get; set; }

        public string Opis { get; set; }

        public string Slika { get; set; }

        public List<MongoDBRef> Knjige { get; set; }

        public Biblioteka()
        {
            Knjige = new List<MongoDBRef>();
        }
    }
}
