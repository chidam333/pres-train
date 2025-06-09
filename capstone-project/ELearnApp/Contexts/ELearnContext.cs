using Microsoft.EntityFrameworkCore;
using ELearnApp.Models;

namespace ELearnApp.Contexts;

public class ElearnContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }

    public ElearnContext(DbContextOptions<ElearnContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoleName).IsRequired();
            entity.HasIndex(e => e.RoleName).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.UserId });
            entity.HasOne(e => e.User).WithMany(u => u.UserCourses).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Course).WithMany(c => c.UserCourses).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.HasOne(e => e.CreatedBy)
            .WithMany(u => u.CreatedCourses)
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Cascade);
            
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.HasOne(e => e.Course).WithMany(c => c.Lessons).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.HasOne(e => e.Lesson).WithMany(l => l.Materials).HasForeignKey(e => e.LessonId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
