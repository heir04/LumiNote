namespace api.Entities
{
    public class Quiz : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid? NoteId { get; set; }
        public Note? Note { get; set; }
        public Guid? SummaryId { get; set; }
        public Summary? Summary { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; } = [];
        public int Score => Questions.Count(q => q.IsAnsweredCorrectly);
        // Small helper for runtime validation
        public bool HasExactlyOneSource() => (NoteId != null) ^ (SummaryId != null);
    }
}