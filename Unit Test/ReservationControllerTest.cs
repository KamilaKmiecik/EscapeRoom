/* 
using Xunit;
using Microsoft.AspNetCore.Mvc;
using EscapeRoom.Controllers;
using EscapeRoom.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using EscapeRoom.Data;
using System.Security.Claims;

namespace EscapeRoom.Tests
{
    public class ReservationsControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly DbContextOptions<EscapeRoomContext> _options;

        public ReservationsControllerTests()
        {
            _options = new DbContextOptionsBuilder<EscapeRoomContext>()
               .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
               .Options;

            // Mocking UserManager<User>
            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task MyReservations_ReturnsViewWithReservationsForCurrentUser()
        {
            // Arrange
            var user = new User { Id = "testUserId", UserName = "testUser" };
            var reservations = new List<Reservation>
            {
                new Reservation { ID = 1, ClientID = "testUserId", Name = "Test Reservation" },
                new Reservation { ID = 2, ClientID = "otherUserId", Name = "Other Reservation" }
            };
            using (var context = new EscapeRoomContext(_options))
            {
                await context.Users.AddAsync(user);
                await context.Reservations.AddRangeAsync(reservations);
                await context.SaveChangesAsync();
            }
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(user)
               .Verifiable();
            var controller = new ReservationsController(new EscapeRoomContext(_options), _mockUserManager.Object);

            // Act
            var result = await controller.MyReservations() as ViewResult;
            var model = result?.Model as IEnumerable<Reservation>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Single(model);
            Assert.True(model.All(r => r.ClientID == user.Id));
            _mockUserManager.Verify();
        }

        [Fact]
        public async Task Index_ReturnsViewWithListOfReservations()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new Reservation { ID = 1, Name = "Test Reservation" },
                new Reservation { ID = 2, Name = "Other Reservation" }
            };
            using (var context = new EscapeRoomContext(_options))
            {
                await context.Reservations.AddRangeAsync(reservations);
                await context.SaveChangesAsync();
            }
            var controller = new ReservationsController(new EscapeRoomContext(_options), _mockUserManager.Object);

            // Act
            var result = await controller.Index() as ViewResult;
            var model = result?.Model as IEnumerable<Reservation>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewWithReservation()
        {
            // Arrange
            var reservation = new Reservation { ID = 1, Name = "Test Reservation" };
            using (var context = new EscapeRoomContext(_options))
            {
                await context.Reservations.AddAsync(reservation);
                await context.SaveChangesAsync();
            }
            var controller = new ReservationsController(new EscapeRoomContext(_options), _mockUserManager.Object);

            // Act
            var result = await controller.Details(1) as ViewResult;
            var model = result?.Model as Reservation;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(1, model.ID);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesReservationAndRedirectsToIndex()
        {
            // Arrange
            var reservation = new Reservation { ID = 1, Name = "Test Reservation" };
            using (var context = new EscapeRoomContext(_options))
            {
                await context.Reservations.AddAsync(reservation);
                await context.SaveChangesAsync();
            }
            var controller = new ReservationsController(new EscapeRoomContext(_options), _mockUserManager.Object);

            // Act
            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            using (var context = new EscapeRoomContext(_options))
            {
                Assert.Null(await context.Reservations.FindAsync(1));
            }
        }
    }
}
*/