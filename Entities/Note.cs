namespace api.Entities
{
    public class Note : BaseEntity
    {
        public string? Title { get; set; }
        public Guid UserId { get; set; }        // FK to User
        public User? User { get; set; }
        public string? Content { get; set; }       // Raw text from note/document
        public string? SourceType { get; set; }    // "Text", "Image", "Audio"
        public string? Summary { get; set; }
    }
}