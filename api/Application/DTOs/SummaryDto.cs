using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}