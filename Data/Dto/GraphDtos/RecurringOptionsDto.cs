using System;
using Nodester.Data.Models.Json_Models;

namespace Nodester.Data.Dto
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