using Xunit;
using EscapeRoom.Controllers;
using EscapeRoom.Models;
using Microsoft.AspNetCore.Mvc;
using EscapeRoom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EscapeRoom.Tests
{
    public class PokojeControllerTests
    {
        [Fact]
        public void DetalePokoi_ReturnsViewWithRoom()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            // Add some test data
            using (var context = new EscapeRoomContext(options))
            {
                context.Rooms.Add(new Room { ID = 1, RoomName = "Test Room", Image = "test.jpg", LongDescription = "This is a test room." });
                context.SaveChanges();
            }

            var controller = new PokojeController(new EscapeRoomContext(options));

            // Act
            var result = controller.DetalePokoi(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as Room;
            Assert.Equal("Test Room", model.RoomName);
        }

        [Fact]
        public void DetalePokoi_ReturnsNotFoundForInvalidRoomId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var controller = new PokojeController(new EscapeRoomContext(options));

            // Act
            var result = controller.DetalePokoi(999) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
