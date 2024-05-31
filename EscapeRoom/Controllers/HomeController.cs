using EscapeRoom.Data;
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

        public IActionResult DashBoard([FromServices] EscapeRoomContext context)
        {
            ViewData["Title"] = "Dashboard";

            ViewBag.Reservations = context.Reservations.ToList();
            ViewBag.Users = context.Users.ToList();
            ViewBag.Rooms = context.Rooms.ToList();
            return View();

        }

        public IActionResult SortedReservations([FromServices] EscapeRoomContext context)
        {
            ViewData["Title"] = "SortedReservations";

            ViewBag.Reservations = context.Reservations.ToList();
            ViewBag.Rooms = context.Rooms.ToList();
            return View();

        }

        
    }
}
