using EscapeRoom.Models;
using EscapeRoom.Data;
using System;
using System.Linq;

namespace YourNamespace.Services
{
    public class ReservationService
    {
        private readonly EscapeRoomContext _context;

        public ReservationService(EscapeRoomContext context)
        {
            _context = context;
        }

        public void CreateWeeklyReservations()
        {
            var rooms = _context.Rooms.ToList();
            var currentDate = DateTime.Now.Date;

            foreach (var room in rooms)
            {
                for (int i = 0; i < 7; i++) // For each day of the week
                {
                    var day = currentDate.AddDays(i);

                    for (int slot = 0; slot < 3; slot++) // 3 slots per day
                    {
                        var reservationStart = day.AddHours(10 + slot * 3); // Example: Slots at 10 AM, 1 PM, and 4 PM
                        var reservationEnd = reservationStart.AddHours(2); // Example: Each slot is 2 hours long

                        var reservation = new Reservation
                        {
                            Name = $"Slot {slot + 1}",
                            ReservationStart = reservationStart,
                            ReservationEnd = reservationEnd,
                            RoomID = room.ID,
                            NumberOfPeople = 0,
                            ClientID = null
                        };

                        _context.Reservations.Add(reservation);
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
