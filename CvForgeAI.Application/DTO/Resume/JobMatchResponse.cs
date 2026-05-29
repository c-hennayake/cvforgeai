using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Application.DTO.Resume
{
    public class JobMatchResponse
    {
        public int MatchPercentage { get; set; }

        public List<string> MissingSkills { get; set; }
            = new();

        public string Suggestions { get; set; }
            = string.Empty;
    }
}
