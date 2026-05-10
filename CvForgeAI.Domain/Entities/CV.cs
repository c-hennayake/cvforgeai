using CvForgeAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Domain.Entities
{
    public class CV : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
    }
}
