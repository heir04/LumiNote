namespace api.Entities
{
    public class Note : BaseEntity
    {
        public Guid UserId { get; set; }        // FK to User
        public string? Content { get; set; }       // Raw text from note/document
        public string? SourceType { get; set; }    // "Text", "Image", "Audio"
    }
}