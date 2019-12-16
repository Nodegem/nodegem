using System;
using Nodegem.Common.Data;

namespace Nodegem.Data.Models.Json_Models
{
    
    public class RecurringOptions
    {
        public FrequencyOptions Frequency { get; set; } = FrequencyOptions.Daily;
        public float Every { get; set; } = 1;
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? Until { get; set; }
        public int? Iterations { get; set; }
    }
}