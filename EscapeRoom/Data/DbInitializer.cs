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

            if (context.Reservations.Any() || context.Rooms.Any() || context.Users.Any())
            {
                return;
            }

            var rooms = new Room[]
            {
                new Room { RoomName="Kosmiczna Wyprawa", IsOccupied=false, Image="zdjecia/KosmicznaWyprawa.jpg", LongDescription="Przygotuj się na niezapomnianą przygodę w naszym najnowszym escape roomie \"Kosmiczna Wyprawa\", gdzie wcielisz się w rolę kosmicznego podróżnika z misją naprawy statku kosmicznego i powrotu na Ziemię przed upływem czasu. Zbieraj wskazówki, rozwiązuj międzygwiezdne zagadki i odkrywaj tajemnicze pomieszczenia, wykorzystując swoją sprytność i zręczność, aby pokonać wyzwania galaktycznej podróży i zapewnić bezpieczny powrót do domu. Czy zdołasz pokonać czas i uwolnić się z kosmicznej pułapki? Odkryj swoje umiejętności w naszym ekscytującym escape roomie już dziś!"},
                new Room { RoomName="Labirynt Złudzeń", IsOccupied=false, Image="zdjecia/LabiryntZludzen.jpg", LongDescription="Intrygujący escape room, w którym gracze muszą pokonać szereg mylących zagadek i iluzji, aby odnaleźć wyjście z zawiłego labiryntu w wyznaczonym czasie."},
                new Room { RoomName="Pokój Tajemnic", IsOccupied=false, Image="zdjecia/PokojTajemnic.jpg", LongDescription="Emocjonująca gra escape room, gdzie drużyna musi rozwiązać zagadki i odkryć ukryte wskazówki, aby wydostać się z tajemniczego pokoju prze upływem czasu."}

            };
            foreach (var room in rooms)
            {
                context.Rooms.Add(room);
            }
            context.SaveChanges();

            var users = new User[]
            {
                new User {  LastName = "Doe", UserType = UserType.Admin },
                new User { LastName = "Smith", UserType = UserType.RoomWorker },
                new User {  LastName = "Johnson", UserType = UserType.DeskWorker },
                new User {  LastName = "Williams", UserType = UserType.Client }
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
                    Client = users.FirstOrDefault(),
                    ClientID = users.FirstOrDefault().Id,
                    NumberOfPeople = 10
                },
                new Reservation
                {
                    Name = "Birthday Party",
                    ReservationStart = DateTime.Now.AddDays(14),
                    ReservationEnd = DateTime.Now.AddDays(14).AddHours(3),
                    RoomID = 2,
                    Client = users.LastOrDefault(), 
                    ClientID = users.LastOrDefault().Id,
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
