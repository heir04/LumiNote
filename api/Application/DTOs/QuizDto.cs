namespace api.Application.DTOs
{
    // For returning quiz data
    public class QuizDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public List<QuizQuestionDto> Questions { get; set; } = [];
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredCount { get; set; }
        public int CorrectlyAnsweredCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // For creating a new quiz
    public class CreateQuizDto
    {
        public string? Title { get; set; }
        public List<CreateQuizQuestionDto> Questions { get; set; } = [];
    }

    // For updating quiz title
    public class UpdateQuizDto
    {
        public string? Title { get; set; }
    }

    // Summary view for listing quizzes
    public class QuizSummaryDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredCount { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
