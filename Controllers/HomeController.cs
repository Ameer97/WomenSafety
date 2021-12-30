using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WomenSafety.Data;
using WomenSafety.Models;

namespace WomenSafety.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetNerstLocations(double lat, double lan, int number = 5)
        {
            var g = "SELECT \"Lat\", \"Lan\", \"Name\", \"TypeId\", \"Description\", ST_Distance(\"Geom\", st_setsrid(ST_MakePoint(" + lan + ", " + lat + "), 4326)) as distance FROM public.\"Points\" order by distance limit " + number + ";";
            //2100
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = g;
            await _context.Database.OpenConnectionAsync();

            var Points = new List<object>();
            using (var rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    try
                    {
                        Points.Add(new
                        {
                            Lat = rdr.GetDouble(0),
                            Lan = rdr.GetDouble(1),
                            Name = rdr.GetString(2),
                            Type = rdr.GetInt32(3),
                            Description = rdr.GetString(4),
                            Distance = rdr.GetDouble(5) * 100,
                        });
                    }
                    catch
                    {
                    }
                }
            }

            return Ok(Points);
        }

        public IActionResult GetThisMapBestDistance()
        {
            return View();
        }
    }
}
