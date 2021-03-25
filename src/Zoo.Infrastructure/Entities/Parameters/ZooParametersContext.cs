using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Zoo.Infrastructure.Entities.Parameters
{
    public partial class ZooParametersContext : DbContext
    {
        public ZooParametersContext(DbContextOptions<ZooParametersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>(entity =>
                                                   {
                                                       entity.ToTable("Configuration");
                                                    
                                                       entity.HasKey(e => e.Key);

                                                       entity.Property(e => e.Key)
                                                             .HasMaxLength(50);
                                                       entity.Property(e => e.Value)
                                                             .IsRequired()
                                                             .HasMaxLength(50);
                                                   });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
