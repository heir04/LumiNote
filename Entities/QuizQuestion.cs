namespace api.Entities
{
    public class QuizQuestion : BaseEntity
    {
        public Guid QuizId { get; set; }           // FK to Quiz
        public string? QuestionText { get; set; }
        public List<string> Options { get; set; } = [];
        public string? Answer { get; set; }
        public string? UserAnswer { get; set; }
        public bool IsAnswered { get; set; } = false;
        public bool IsAnsweredCorrectly { get; set; } = false;
        public string? Explanation { get; set; } 
    }
}