using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;

namespace SchoolApp.Data;

public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; init; }
    public DbSet<Enrollment> Enrollments { get; init; }
    public DbSet<Group> Groups { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Group>().ToTable("Group");
        modelBuilder.Entity<Student>().ToTable("Student");
    }
}