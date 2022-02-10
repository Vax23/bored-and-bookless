using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoredAndBookless.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BoredAndBookless.Models
{
    public class Rezervacija
    {
        public ObjectId Id { get; set; }

        public MongoDBRef Korisnik { get; set; }

        public MongoDBRef Knjiga { get; set; }

        public DateTime DatumUzimanja { get; set; }

        public DateTime DatumVracanja { get; set; }
    }
}
