using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options): base(options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Student>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Student>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Entity<Student>()
                .Property(p => p.Address)
                .HasMaxLength(50)
                .IsRequired();
            builder.Entity<Student>()
                .Property(p => p.PhoneNumber)
                .HasMaxLength(10)
                .IsRequired();

            builder.Entity<User>()
                .Property(p => p.Id);
            builder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Password)
                .IsRequired();
            builder.Entity<User>()
                .Property(p => p.Password)
                .IsRequired();
        }
    }
}
