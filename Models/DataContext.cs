using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tpaoProjeMvc.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Saha> Sahalar { get; set; }
    public DbSet<Kuyu> Kuyular { get; set; }
    public DbSet<Wellbore> Wellbores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Saha>()
            .HasMany(s => s.kuyuList)
            .WithOne(k => k.Saha)
            .HasForeignKey(k => k.SahaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Kuyu>()
            .HasMany(k => k.wellbores)
            .WithOne(w => w.Kuyu)
            .HasForeignKey(w => w.KuyuId)
            .OnDelete(DeleteBehavior.Cascade);
    }





}
