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

        // GET: Reservation/Day
        public IActionResult Day(string date)
        {
            if (!DateTime.TryParse(date, out var reservationDate))
            {
                return BadRequest("Invalid date format");
            }

            ViewBag.ReservationDate = reservationDate;

            return View(); 
        }
    }
}
