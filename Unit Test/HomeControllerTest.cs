using Xunit;
using EscapeRoom.Controllers;
using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;
using EscapeRoom.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EscapeRoom.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsView()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ViewData["Title"]);
        }

        [Fact]
        public void Kontakt_ReturnsView()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Kontakt() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Kontakt", result.ViewData["Title"]);
        }

        [Fact]
        public void DashBoard_ReturnsViewWithModelData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            // Add some test data
            using (var context = new EscapeRoomContext(options))
            {
                context.Reservations.AddRange(
                    new Reservation { ID = 1, Name = "Reservation 1" },
                    new Reservation { ID = 2, Name = "Reservation 2" }
                );

                context.Users.AddRange(
                    new User { Id = "1", UserName = "User 1" },
                    new User { Id = "2", UserName = "User 2" }
                );

                context.Rooms.AddRange(
                    new Room { ID = 1, RoomName = "Room 1", Image = "Image1.jpg", LongDescription = "tttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" },
                    new Room { ID = 2, RoomName = "Room 2" , Image = "Image1.jpg", LongDescription = "tttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" }
                );

                context.SaveChanges();
            }

            var controller = new HomeController();

            // Act
            var result = controller.DashBoard(new EscapeRoomContext(options)) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ViewData["Title"]);

            var reservations = result.ViewData["Reservations"] as List<Reservation>;
            Assert.NotNull(reservations);
            Assert.Equal(2, reservations.Count);

            var users = result.ViewData["Users"] as List<User>;
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);

            var rooms = result.ViewData["Rooms"] as List<Room>;
            Assert.NotNull(rooms);
            Assert.Equal(2, rooms.Count);
        }

        [Fact]
        public void SortedReservations_ReturnsViewWithModelData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            // Add some test data
            using (var context = new EscapeRoomContext(options))
            {
                context.Reservations.AddRange(
                    new Reservation { ID = 1, Name = "Reservation 1" },
                    new Reservation { ID = 2, Name = "Reservation 2" }
                );

                context.Rooms.AddRange(
                    new Room { ID = 1, RoomName = "Room 1", Image = "Image1.jpg", LongDescription = "tttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" },
                    new Room { ID = 2, RoomName = "Room 2", Image = "Image1.jpg", LongDescription = "tttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt" }
                );

                context.SaveChanges();
            }

            var controller = new HomeController();

            // Act
            var result = controller.SortedReservations(new EscapeRoomContext(options)) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SortedReservations", result.ViewData["Title"]);

            var reservations = result.ViewData["Reservations"] as List<Reservation>;
            Assert.NotNull(reservations);
            Assert.Equal(2, reservations.Count);

            var rooms = result.ViewData["Rooms"] as List<Room>;
            Assert.NotNull(rooms);
            Assert.Equal(2, rooms.Count);
        }
    }
}
