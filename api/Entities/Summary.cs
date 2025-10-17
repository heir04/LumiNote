using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Summary : BaseEntity
    {
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? Content { get; set; }
    }
}