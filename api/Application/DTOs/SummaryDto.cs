using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.Application.DTOs
{
    public class SummaryDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSummaryDto
    {
        public string? Content { get; set; }
        public string? Title { get; set; }
    }

    public class PdfQuestion
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> Options { get; set; }
        public string? Explanation { get; set; }
    }
}