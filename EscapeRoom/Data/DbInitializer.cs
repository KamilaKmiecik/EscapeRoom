using EscapeRoom.Models;
using System;
using System.Linq;

namespace EscapeRoom.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EscapeRoomContext context)
        {
            context.Database.EnsureCreated();

            if (context.Reservations.Any())
            {
                return;
            }

            var rooms = new Room[]
            {
                new Room { RoomName = "Room 1", IsOccupied = false },
                new Room { RoomName = "Room 2", IsOccupied = false },
                new Room { RoomName = "Room 3", IsOccupied = false }
            };
            foreach (var room in rooms)
            {
                context.Rooms.Add(room);
            }
            context.SaveChanges();

            var users = new User[]
            {
                new User { FirstName = "John", LastName = "Doe", UserType = UserType.Admin },
                new User { FirstName = "Jane", LastName = "Smith", UserType = UserType.RoomWorker },
                new User { FirstName = "Alice", LastName = "Johnson", UserType = UserType.DeskWorker },
                new User { FirstName = "Bob", LastName = "Williams", UserType = UserType.Client }
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            var reservations = new Reservation[]
            {
                new Reservation
                {
                    Name = "Team Building Event",
                    ReservationStart = DateTime.Now.AddDays(7),
                    ReservationEnd = DateTime.Now.AddDays(8),
                    RoomID = 1,
                    ClientID = 4,
                    NumberOfPeople = 10
                },
                new Reservation
                {
                    Name = "Birthday Party",
                    ReservationStart = DateTime.Now.AddDays(14),
                    ReservationEnd = DateTime.Now.AddDays(14).AddHours(3),
                    RoomID = 2,
                    ClientID = 4,
                    NumberOfPeople = 8
                }
            };
            foreach (var reservation in reservations)
            {
                context.Reservations.Add(reservation);
            }
            context.SaveChanges();
        }
    }
}
