using EscapeRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EscapeRoom.Data;

public class EscapeRoomContext : IdentityDbContext<User>
{
    public EscapeRoomContext(DbContextOptions<EscapeRoomContext> options) : base(options)
    {
    }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Reservation>().ToTable("Reservations");
        modelBuilder.Entity<Room>().ToTable("Rooms");
        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<Reservation>()
             .HasOne(r => r.Room)
             .WithMany()
             .HasForeignKey(r => r.RoomID);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.ClientID);
    }
}

