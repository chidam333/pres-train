namespace twitter_like.Models;
public class Tweet
{
    public int Id { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new User();
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Retweet>? Retweets { get; set; }
}