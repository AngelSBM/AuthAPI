using Auth.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<User>()
            //    .ToTable("dbo.USERS")  
            //    .Property(e => e.Name).HasColumnName("NAME")
            //    .HasKey(e => e.Id);

            modelBuilder.Entity<User>( entity =>
            {
                entity.ToTable("USERS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("PERSON_NAME");
                entity.Property(e => e.LastName).HasColumnName("LAST_NAME");
                entity.Property(e => e.BirthDate).HasColumnName("BIRTH_DATE");
                entity.Property(e => e.Email).HasColumnName("EMAIL");
                entity.Property(e => e.Password).HasColumnName("USER_PASSWORD");
                entity.Property(e => e.PublicId).HasColumnName("PUBLIC_ID");
                entity.Property(e => e.Salt).HasColumnName("SALT");
            } );

        }        
    }
}
