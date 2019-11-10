using System;
using Nodester.Common.Data;

namespace Nodester.Common.Dto
{
    public class RecurringOptionsDto
    {
        public FrequencyOptions Frequency { get; set; }
        public float Every { get; set; }
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? Until { get; set; }
        public int Iterations { get; set; }
    }
}