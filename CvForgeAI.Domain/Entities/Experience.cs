using CvForgeAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Domain.Entities
{
    public class Experience : BaseEntity
    {
        public Guid CVId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public CV CV { get; set; } = null!;
    }
}
