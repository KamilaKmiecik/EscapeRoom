using EscapeRoom.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EscapeRoom.Data;

public class EscapeRoomContext : DbContext
{
    public EscapeRoomContext(DbContextOptions<EscapeRoomContext> options) : base(options)
    {
    }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>().ToTable("Reservations");
        modelBuilder.Entity<Room>().ToTable("Rooms");
        modelBuilder.Entity<User>().ToTable("Users");

    }
}

