﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class ScheduleService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public ScheduleService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var nextRunTime = GetNextRunTime();
            var timeToGo = nextRunTime - DateTime.Now;
            _timer = new Timer(DoWork, null, timeToGo, Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        private DateTime GetNextRunTime()
        {
            DateTime nextRunTime = DateTime.Today.AddDays(((int)DayOfWeek.Sunday - (int)DateTime.Today.DayOfWeek + 7) % 7).AddHours(20).AddMinutes(40); //  Monday 10 AM
            if (nextRunTime < DateTime.Now)
            {
                nextRunTime = nextRunTime.AddDays(7);
            }
            return nextRunTime;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var reservationService = scope.ServiceProvider.GetRequiredService<ReservationService>();
                reservationService.CreateSlotsForSixWeeks();
            }
            _timer.Change(GetNextRunTime() - DateTime.Now, Timeout.InfiniteTimeSpan);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
