using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Domain.Entities
{
    public class Education
    {
        public int Id { get; set; }

        public string InstituteName { get; set; } = string.Empty;

        public string Degree { get; set; } = string.Empty;

        public string FieldOfStudy { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrentlyStudying { get; set; }

        public int ResumeId { get; set; }

        public Resume Resume { get; set; } = null!;
    }
}

