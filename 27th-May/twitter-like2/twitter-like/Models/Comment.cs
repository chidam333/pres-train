namespace twitter_like.Models;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int TweetId { get; set; }
    public Tweet? Tweet { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}