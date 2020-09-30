using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityBestEmployees.MS.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CityBestEmployees.MS.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        [Route("Index")]
        public IActionResult Index(DashBoardDto model)
        {

            ViewBag.IsLoggedIn = model.Role.Contains("Admin");
            ViewBag.IsAdmin = true;
            return View(model);
        }
 
    }
}
