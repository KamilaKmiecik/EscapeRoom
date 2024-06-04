using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EscapeRoom.Data;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace EscapeRoom.Controllers
{
    public class RezerwacjeController : Controller
    {
        private readonly EscapeRoomContext _context;
        private readonly UserManager<User> _userManager;

        public RezerwacjeController(EscapeRoomContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var reservationDate))
            {
                return BadRequest("Invalid date format");
            }

            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.ReservationStart.Date == reservationDate.Date)
                .OrderBy(r => r.Room.RoomName)
                .ThenBy(r => r.ReservationStart)
                .ToListAsync();

            ViewBag.ReservationDate = reservationDate;
            ViewBag.AvailableSlots = reservations;
            ViewBag.Rooms = _context.Rooms;

            return View();
        }

        [HttpGet("api/Reservations/availability")]
        public async Task<IActionResult> GetAvailability()
        {
            try
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
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error fetching availability: " + ex.Message);
                return StatusCode(500, "Error fetching availability");
            }
        }

        [HttpPost("api/Reservations/ReserveSlot")]
        public async Task<IActionResult> ReserveSlot([FromBody] ReservationRequest request)
        {
            //this.User.Identity.
            try
            {
                var reservation = await _context.Reservations.FindAsync(request.SlotID);
                if (reservation == null)
                {
                    return NotFound("Slot not found");
                }

                reservation.NumberOfPeople = request.NumberOfPeople;

                _context.Update(reservation);
                await _context.SaveChangesAsync();

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized("User not logged in");
                }

                reservation.ClientID = user.Id;
                reservation.Client = user;

                _context.Update(reservation);
                await _context.SaveChangesAsync();

                return Ok("Slot reserved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while reserving slot: {ex.Message}");
            }
        }
    }
}
