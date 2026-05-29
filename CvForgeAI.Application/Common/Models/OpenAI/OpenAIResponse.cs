using System;
using System.Collections.Generic;
using System.Text;

namespace CvForgeAI.Application.Common.Models.OpenAI
{
    public class OpenAIResponse
    {
        public List<Choice> Choices { get; set; }
            = new();
    }
}
