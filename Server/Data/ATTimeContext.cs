using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CrownATTime.Server.Models.ATTime;

namespace CrownATTime.Server.Data
{
    public partial class ATTimeContext : DbContext
    {
        public ATTimeContext()
        {
        }

        public ATTimeContext(DbContextOptions<ATTimeContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.IsNonBillable)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.ShowOnInvoice)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.IsCompleted)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.TimeStampStatus)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.DateWorked)
              .HasColumnType("datetimeoffset");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.StartDateTime)
              .HasColumnType("datetimeoffset");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.EndDateTime)
              .HasColumnType("datetimeoffset");
            this.OnModelBuilding(builder);
        }

        public DbSet<CrownATTime.Server.Models.ATTime.TimeEntry> TimeEntries { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.SqlServerOnDeleteConvention));
        }
    }
}