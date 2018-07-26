namespace WhiteRaven.Domain.Models.Note
{
    public class Commit
    {
        public string Title { get; }
        public string Content { get; }

        public Commit(string title, string content)
        {
            Title = title ?? string.Empty;
            Content = content ?? string.Empty;
        }
    }
}