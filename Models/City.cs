using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WomenSafety.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Geometry Geom { get; set; }
        public string TextFormat { get; set; }
        public string TextPoly { get; set; }
    }
}
