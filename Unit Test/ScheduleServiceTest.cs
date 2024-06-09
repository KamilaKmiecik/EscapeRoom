using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using EscapeRoom.Data;
using EscapeRoom.Models;
using YourNamespace.Services;

namespace YourNamespace.Tests
{
    public class ScheduleServiceTests
    {
        [Fact]
        public async Task ScheduleService_ExecutesDoWork()
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

            var serviceProviderMock = new Mock<IServiceProvider>();
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            var serviceScopeMock = new Mock<IServiceScope>();

            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                               .Returns(scopeFactoryMock.Object);

            // Create a ReservationService instance with the context
            using (var context = new EscapeRoomContext(options))
            {
                var reservationService = new ReservationService(context);
                serviceProviderMock.Setup(x => x.GetService(typeof(ReservationService)))
                                   .Returns(reservationService);

                var scheduleService = new ScheduleService(serviceProviderMock.Object);

                // Act
                await scheduleService.StartAsync(CancellationToken.None);

                // Simulate timer callback
                var privateObject = new PrivateObject(scheduleService);
                privateObject.Invoke("DoWork", new object[] { null });

                // Assert
                serviceProviderMock.Verify(x => x.GetService(typeof(ReservationService)), Times.Once);
            }
        }
    }

    // Helper class to access private members in tests
    public class PrivateObject
    {
        private readonly object _instance;
        private readonly Type _type;

        public PrivateObject(object instance)
        {
            _instance = instance;
            _type = instance.GetType();
        }

        public object Invoke(string methodName, object[] args)
        {
            var method = _type.GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return method.Invoke(_instance, args);
        }
    }
}
