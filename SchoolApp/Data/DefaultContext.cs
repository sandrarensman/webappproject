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
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Enrollment>()
            .ToTable("Enrollment")
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e=>e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Group>()
            .ToTable("Group")
            .HasMany(g => g.Students)
            .WithMany(s => s.Groups)
            .UsingEntity<Dictionary<string, object>>(
                "GroupStudent",
                j => j
                    .HasOne<Student>()
                    .WithMany()
                    .HasForeignKey("StudentId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Group>()
                    .WithMany()
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.Cascade)
            );
    }
}