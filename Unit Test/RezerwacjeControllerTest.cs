using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EscapeRoom.Controllers;
using EscapeRoom.Data;
using EscapeRoom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Text.Json;

namespace EscapeRoom.Tests
{
    public class ErrorMessage { public string message { get; set; } }
    public class RezerwacjeControllerTests
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

            context.Reservations.AddRange(
                new Reservation { Name = "Pokój 1", ID = 1, RoomID = 1, ReservationStart = DateTime.Now.Date.AddHours(10), ReservationEnd = DateTime.Now.Date.AddHours(12), NumberOfPeople = 2, ClientID = "1" },
                new Reservation { Name = "Pokój 1", ID = 2, RoomID = 1, ReservationStart = DateTime.Now.Date.AddHours(13), ReservationEnd = DateTime.Now.Date.AddHours(15), NumberOfPeople = 4, ClientID = null }
            );
            context.SaveChanges();
            return context;
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User { Id = "1", UserName = "testuser" });

            return userManager;
        }

        [Fact]
        public void Rezerwacje_ReturnsViewResult()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);

            // Act
            var result = controller.Rezerwacje();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Rezerwacje", viewResult.ViewData["Title"]);
        }

        [Fact]
        public void DetalePokoi_ReturnsViewResult()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);

            // Act
            var result = controller.DetalePokoi();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("DetalePokoi", viewResult.ViewData["Title"]);
        }

        [Fact]
        public async Task Day_ReturnsBadRequest_WhenDateFormatIsInvalid()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);

            // Act
            var result = await controller.Day("invalid-date");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date format", badRequestResult.Value);
        }

        /*
        [Fact]
        public async Task GetAvailability_ReturnsOkResult_WithAvailability()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);

            // Act
            var result = await controller.GetAvailability();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var availability = Assert.IsType<Dictionary<string, List<object>>>(okResult.Value);
            Assert.Single(availability);
        }
        
        [Fact]
        
        public async Task ReserveSlot_ReturnsNotFound_WhenRequestIsInvalid()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);
            var request = new ReservationRequest { SlotID = 1, NumberOfPeople = 0 };

            // Act
            var result = await controller.ReserveSlot(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(404, jsonResult.StatusCode);

            // Access the message property dynamically
            dynamic value = jsonResult.Value;
            Assert.Equal("Wybierz ilość osób do rezerwacji!", (string)value.message);
        }



        [Fact]
        public async Task ReserveSlot_ReturnsNotFound_WhenSlotNotFound()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);
            var request = new ReservationRequest { SlotID = 99, NumberOfPeople = 1 };

            // Act
            var result = await controller.ReserveSlot(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(404, jsonResult.StatusCode);
           
            Assert.Equal("Slot not found", ((dynamic)jsonResult.Value).message);
        }

        [Fact]
        public async Task ReserveSlot_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManagerMock = GetMockUserManager();
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);
            var userManager = userManagerMock.Object;
            var controller = new RezerwacjeController(context, userManager);
            var request = new ReservationRequest { SlotID = 2, NumberOfPeople = 1 };

            // Act
            var result = await controller.ReserveSlot(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(401, jsonResult.StatusCode);
           
            Assert.Equal("Proszę zaloguj się przed stworzeniem rezerwacji!", ((dynamic)jsonResult.Value).message);
        }

        [Fact]
        public async Task ReserveSlot_ReturnsSuccessMessage_WhenReservationIsSuccessful()
        {
            // Arrange
            using var context = GetContextWithData();
            var userManager = GetMockUserManager().Object;
            var controller = new RezerwacjeController(context, userManager);
            var request = new ReservationRequest { SlotID = 2, NumberOfPeople = 1 };

            // Act
            var result = await controller.ReserveSlot(request);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            
            Assert.Equal("Rezerwacja przebiegła pomyślnie", ((dynamic)jsonResult.Value).message);
        }*/
    }
}