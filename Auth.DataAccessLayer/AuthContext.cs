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
        public DbSet<UserRole> UsersRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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

            modelBuilder.Entity<RefreshToken>(entity =>
           {
               entity.ToTable("USER_SESSIONS");
               entity.HasKey(e => e.Id);
               entity.Property(e => e.Id).HasColumnName("ID");
               entity.Property(e => e.Token).HasColumnName("REFRESH_TOKEN");
               entity.Property(e => e.UserId).HasColumnName("USER_ID");
               entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
               entity.Property(e => e.ExpiresAt).HasColumnName("EXPIRES_AT");
           } );


            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("NAME");
                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");
                entity.Property(e => e.PublicId).HasColumnName("PUBLIC_ID");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("USER_ROLES");
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Id).HasColumnName("ID");
                entity.Property(entity => entity.UserId).HasColumnName("USERID");
                entity.Property(entity => entity.RoleId).HasColumnName("ROLID");

                entity.HasOne<User>(u => u.User).WithMany(e => e.UsersRoles);

                entity.HasOne<Role>(u => u.Role).WithMany(e => e.UsersRoles);


            });

        }        
    }
}
