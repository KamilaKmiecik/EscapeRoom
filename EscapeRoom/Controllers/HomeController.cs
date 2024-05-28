using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscapeRoom.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";

            return View();
        }

        public IActionResult Kontakt()
        {
            ViewData["Title"] = "Kontakt";
            return View();
        }

    }
}
