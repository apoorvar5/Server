using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CountryModel;

public partial class PlayerSourceContext : IdentityDbContext<ClubPlayerUser>
{
    public PlayerSourceContext()
    {
    }

    public PlayerSourceContext(DbContextOptions<PlayerSourceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<PlayerClub> PlayerClub { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK_tmp_ms_x_AF82112AB7EE1730");
        });

        modelBuilder.Entity<PlayerClub>(entity =>
        {
            entity.HasOne(d => d.Club).WithMany(p => p.PlayerClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerClub_Club");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerClub_Player");

        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK_tmp_ms_x_757BC9A0CB74E8F6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}