using Microsoft.EntityFrameworkCore;

public class FaqContext : DbContext
{
    public FaqContext(DbContextOptions<FaqContext> options) : base(options)
    {
    }

    public DbSet<BotRequest> BotRequests { get; set; }
    public DbSet<BotResponse> BotResponses { get; set; }

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
                entity.HasOne(e => e.Feedback);
            }
        );

        modelBuilder.Entity<Feedback>(
            entity =>
            {
                entity.HasKey(e => e.FeedbackId);
                entity.Property(e => e.Value).IsRequired();
            }
        );
    }
}