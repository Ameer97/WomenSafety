using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            Task.Run(() => CreateNewRoleWithUSer()).Wait();
        }

        private async Task CreateNewRoleWithUSer()
        {
            await CreateNewRoleWithUSerProccess(SystemRoles.All());
        }

        private async Task CreateNewRoleWithUSerProccess(List<string> RoleNames)
        {
            foreach (var RoleName in RoleNames)
            {
                if (await _roleManager.RoleExistsAsync(RoleName))
                    return;

                var role = new IdentityRole { Name = RoleName };
                var IdentityResult = await _roleManager.CreateAsync(role);

                if (IdentityResult.Succeeded)
                {
                    var user = new IdentityUser { UserName = RoleName.ToLower() + "@email.com", Email = RoleName.ToLower() + "@email.com" };
                    string userPWD = "123qwe!@#QWE";

                    var chkUser = await _userManager.CreateAsync(user, userPWD);

                    if (chkUser.Succeeded) await _userManager.AddToRoleAsync(user, RoleName);

                }
            }
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cases()
        {
            return View();
        }

        public async Task<IActionResult> GetCases()
        {
            //var f = new List<int>();
            //var random = new Random();
            //var first = 20;
            //var last = 65;
            //for (int i = 0; i < 1000; i++)
            //{
            //    f.Add(random.Next(first, last));
            //}
            //var d = f.GroupBy(a => a).Select(a => new Case
            //{
            //    Id = a.Key.ToString(),
            //    Count = a.Count()
            //}).ToList();

            //var result = new List<Case>();
            //var min = 0;
            //var max = 0;
            //for (int i = first; i < last; i+=10)
            //{
            //    min = int.Parse(i.ToString().First() + "0");
            //    max = min + 10;
            //    result.Add(new Case
            //    {
            //        Count = d.Where(m => int.Parse(m.Id) >= min && int.Parse(m.Id) <= max).Select(a => a.Count).Sum()
            //    });
            //}

            //result = result.OrderByDescending(a => a.Count).ToList();
            //var counter = 0;
            //for (int i = first; i < last; i+=10)
            //{
            //    min = int.Parse(i.ToString().First() + "0");
            //    max = min + 10;
            //    result[counter].Id = "from" + min + "to" + max;
            //    counter++;
            //}

            var result = new List<Case>
            {
                new Case{ Id = "From 20 to 30", Count = 20},
                new Case{ Id = "From 20 to 30", Count = 27},
                new Case{ Id = "From 30 to 40", Count = 22},
                new Case{ Id = "From 40 to 50", Count = 15},
                new Case{ Id = "From 50 to 60", Count = 11},
                new Case{ Id = "From 60 to 70", Count = 5},
            };

            return Ok(result);
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
