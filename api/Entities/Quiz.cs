namespace api.Entities
{
    public class Quiz : BaseEntity
    {
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid NoteId { get; set; }
        public Note? Note { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; } = [];
        public int Score => Questions.Count(q => q.IsAnsweredCorrectly);
    }
}