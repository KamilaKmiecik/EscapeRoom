using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EscapeRoom.Controllers;
using EscapeRoom.Data;
using EscapeRoom.Models;

namespace EscapeRoom.Tests
{
    public class RoomsControllerTests
    {
        private EscapeRoomContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<EscapeRoomContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new EscapeRoomContext(options);
            context.Rooms.AddRange(
                new Room { ID = 1, RoomName = "Room 1", IsOccupied = false, LongDescription = "Description 1", Image = "Image1.jpg" },
                new Room { ID = 2, RoomName = "Room 2", IsOccupied = false, LongDescription = "Description 2", Image = "Image2.jpg" }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfRooms()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Room>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithRoom()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Room>(viewResult.ViewData.Model);
            Assert.Equal(1, model.ID);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenRoomNotFound()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Details(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectAndAddsRoom()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);
            var room = new Room { ID = 3, RoomName = "Room 3", IsOccupied = false, LongDescription = "Description 3", Image = "Image3.jpg" };

            // Act
            var result = await controller.Create(room);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(3, context.Rooms.Count());
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithRoom()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Room>(viewResult.ViewData.Model);
            Assert.Equal(1, model.ID);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenRoomNotFound()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithRoom()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Room>(viewResult.ViewData.Model);
            Assert.Equal(1, model.ID);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRoomNotFound()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesRoomAndRedirects()
        {
            // Arrange
            using var context = GetContextWithData();
            var controller = new RoomsController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(1, context.Rooms.Count());
        }
    }
}
