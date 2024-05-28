using EscapeRoom.Data;
using Microsoft.AspNetCore.Mvc;

namespace EscapeRoom.Controllers
{
    public class PokojeController : Controller
    {
        private readonly EscapeRoomContext _context;

        public PokojeController(EscapeRoomContext context)
        {
            _context = context;
        }

        public IActionResult DetalePokoi(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.ID == id);

            if (room == null)
            {
                return NotFound();
            }

            ViewData["Title"] = room.RoomName;

            return View(room);
        }
    }
}
