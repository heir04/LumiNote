using Microsoft.EntityFrameworkCore;
using api.Entities;

namespace api.Infrastructure.Context
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<QuizQuestion>? QuizQuestions { get; set; }
        public DbSet<Note>? Notes { get; set; }
        public DbSet<User>? Users { get; set; }
    }
}