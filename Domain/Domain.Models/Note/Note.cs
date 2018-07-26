using System;

namespace WhiteRaven.Domain.Models.Note
{
    public class Note
    {
        public string Id { get; }

        public DateTime CreatedUtc { get; }
        public DateTime LastModifiedUtc { get; }

        public string Title { get; }

        public string Content { get; }


        public Note(string id, DateTime createdUtc, DateTime lastModifiedUtc, string title, string content)
        {
            Id = id;
            CreatedUtc = createdUtc;
            LastModifiedUtc = lastModifiedUtc;

            Title = title;
            Content = content;
        }
        
        public Note UpdateTitle(string newTitle) =>
            new Note(Id, CreatedUtc, DateTime.UtcNow, newTitle, Content);

        public Note UpdateContent(string newContent) =>
            new Note(Id, CreatedUtc, DateTime.UtcNow, Title, newContent);

        public Note UpdateTitleAndContent(string newTitle, string newContent) =>
            new Note(Id, CreatedUtc, DateTime.UtcNow, newTitle, newContent);

        public static Note Create(string title, string content) =>
            new Note(Guid.NewGuid().ToString("N"), DateTime.UtcNow, DateTime.UtcNow, title, content);
    }
}