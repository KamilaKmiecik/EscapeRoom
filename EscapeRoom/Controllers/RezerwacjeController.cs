using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EscapeRoom.Data;

namespace EscapeRoom.Controllers
{
    public class RezerwacjeController : Controller
    {
        private readonly EscapeRoomContext _context;

        public RezerwacjeController(EscapeRoomContext context)
        {
            _context = context;
        }

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
        public async Task<IActionResult> Day(string date)
        {
            if (!DateTime.TryParse(date, out var reservationDate))
            {
                return BadRequest("Invalid date format");
            }

            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.ReservationStart.Date == reservationDate.Date)
                .ToListAsync();

            var rooms = await _context.Rooms.ToListAsync();

            var roomAvailability = rooms.Select(room => new
            {
                Room = room,
                Status = reservations.Any(r => r.RoomID == room.ID && r.ClientID != null) ? "reserved" : "available"
            }).ToList();

            ViewBag.ReservationDate = reservationDate;
            ViewBag.RoomAvailability = roomAvailability;

            return View();
        }

        [HttpGet("api/Reservations/availability")]
        public async Task<IActionResult> GetAvailability()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();

            var availability = reservations
                .GroupBy(r => r.ReservationStart.Date)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => g.Select(r => new
                    {
                        name = r.Room.RoomName,
                        status = r.ClientID == null ? "available" : "reserved"
                    }).ToList()
                );

            return Ok(availability);
        }
    }
}
