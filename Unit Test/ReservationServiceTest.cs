using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EscapeRoom.Models;
using EscapeRoom.Data;
using YourNamespace.Services;

namespace YourNamespace.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void CreateSlotsForSixWeeks_CreatesCorrectNumberOfSlots()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            using (var context = new EscapeRoomContext(options))
            {
                context.Rooms.Add(new Room
                {
                    ID = 1,
                    RoomName = "Room 1",
                    Image = "image.jpg",
                    LongDescription = "A long description of the room."
                });
                context.SaveChanges();
            }

            using (var context = new EscapeRoomContext(options))
            {
                var service = new ReservationService(context);

                // Act
                service.CreateSlotsForSixWeeks();

                // Assert
                var reservations = context.Reservations.ToList();
                Assert.Equal(126, reservations.Count); // 6 weeks * 7 days * 3 slots per day
            }
        }

        [Fact]
        public void CreateSlotsForSixWeeks_DoesNotCreateDuplicateSlots()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            using (var context = new EscapeRoomContext(options))
            {
                context.Rooms.Add(new Room
                {
                    ID = 1,
                    RoomName = "Room 1",
                    Image = "image.jpg",
                    LongDescription = "A long description of the room."
                });
                context.SaveChanges();
            }

            using (var context = new EscapeRoomContext(options))
            {
                var service = new ReservationService(context);
                service.CreateSlotsForSixWeeks(); // First call to create slots

                // Act
                service.CreateSlotsForSixWeeks(); // Second call should not create duplicate slots

                // Assert
                var reservations = context.Reservations.ToList();
                Assert.Equal(126, reservations.Count); // Still should be 126
            }
        }
    }
}
