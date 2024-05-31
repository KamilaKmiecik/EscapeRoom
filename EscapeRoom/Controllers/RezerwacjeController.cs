using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;

namespace EscapeRoom.Controllers
{
    public class RezerwacjeController : Controller
    {
        [Route("Rezerwacje")]
        public IActionResult Rezerwacje()
        {
            ViewData["Title"] = "Rezerwacje";
            return View();
        }

        [Route("DetalePokoi")]
        public IActionResult DetalePokoi()
        {
            ViewData["Title"] = "DetalePokoi";
            return View();
        }

        // GET: Reservation/Day
        public IActionResult Day(string date, Room room)
        {
            if (!DateTime.TryParse(date, out var reservationDate))
            {
                return BadRequest("Invalid date format");
            }

            ViewBag.ReservationDate = reservationDate;
            ViewBag.Room = room;

            return View(); 
        }
    }
}
