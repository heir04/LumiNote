namespace api.Application.DTOs
{
    // For returning note data
    public class NoteDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public string? SourceType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // For creating a new note
    public class CreateNoteDto
    {
        public string? Content { get; set; }
        public string? SourceType { get; set; }  // "Text", "Image", "Audio"
    }

    // For updating a note
    public class UpdateNoteDto
    {
        public string? Content { get; set; }
    }
}
