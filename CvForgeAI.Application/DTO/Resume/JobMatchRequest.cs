using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Application.DTO.Resume
{
    public class JobMatchRequest
    {
        public int ResumeId { get; set; }

        public string JobDescription { get; set; }
            = string.Empty;
    }
}
