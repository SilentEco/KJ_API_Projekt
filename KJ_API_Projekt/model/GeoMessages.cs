using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.model
{
    public class GeoMessages
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }
    }
}
