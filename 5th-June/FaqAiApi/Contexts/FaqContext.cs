using microsoft.entityframeworkcore;
using faqaiapi.models;

namespace faqaiapi.contexts;

public class FaqContext : DbContext
{
    public FaqContext(DbContextOptions<FaqContext> options) : base(options)
    {
    }

    public DbSet<BotRequest> BotRequests { get; set; }
    public DbSet<BotResponse> BotResponses { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BotRequest>(
            entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.Property(e => e.UserMessage).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.SessionId).IsRequired();
                entity.HasOne(e => e.Session)
                    .WithMany(s => s.BotRequests)
                    .HasForeignKey(s => s.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        );

        modelBuilder.Entity<BotResponse>(
            entity =>
            {
                entity.HasKey(e => e.ResponseId);
                entity.Property(e => e.BotMessage).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.SessionId).IsRequired();
                entity.HasOne(e => e.Session)
                    .WithMany(s => s.BotResponses)
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Feedback)
                    .WithMany()
                    .HasForeignKey(e => e.FeedbackId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            }
        );

        modelBuilder.Entity<Feedback>(
            entity =>
            {
                entity.HasKey(e => e.FeedbackId);
                entity.Property(e => e.Value).IsRequired();
            }
        );

        modelBuilder.Entity<Session>(
            entity =>
            {
                entity.HasKey(e => e.SessionId);
                entity.HasMany(e => e.BotRequests)
                    .WithOne(req => req.Session)
                    .HasForeignKey(req => req.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.BotResponses)
                    .WithOne(res => res.Session)
                    .HasForeignKey(res => res.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Feedbacks)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            }
        );
    }
}