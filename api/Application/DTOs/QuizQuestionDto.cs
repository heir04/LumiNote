namespace api.Application.DTOs
{
    // For returning quiz question data
    public class QuizQuestionDto
    {
        public Guid QuizId { get; set; }
        public string? QuestionText { get; set; }
        public List<string> Options { get; set; } = [];
        public string? Answer { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsAnsweredCorrectly { get; set; }
        public string? Explanation { get; set; }
        public string? UserAnswer { get; set; }
    }

    // For displaying questions during quiz (without answer)
    public class QuizQuestionDisplayDto
    {
        public Guid QuizId { get; set; }
        public int QuestionNumber { get; set; }  // For displaying "Question 1 of 10"
        public string? QuestionText { get; set; }
        public List<string> Options { get; set; } = [];
        public bool IsAnswered { get; set; }
        public string? UserAnswer { get; set; }  // Show what they selected if already answered
    }

    // For creating a new quiz question
    public class CreateQuizQuestionDto
    {
        public string? QuestionText { get; set; }
        public List<string> Options { get; set; } = new();
        public string? Answer { get; set; }
        public string? Explanation { get; set; }
    }

    // For submitting an answer to a question
    public class SubmitAnswerDto
    {
        public string? UserAnswer { get; set; }
    }

    // For returning answer result
    public class AnswerResultDto
    {
        public bool IsCorrect { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? Explanation { get; set; }
    }
}
