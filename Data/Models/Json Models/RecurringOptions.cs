using System;
using System.Runtime.Serialization;

namespace Nodester.Data.Models.Json_Models
{

    public enum FrequencyOptions
    {
        [EnumMember(Value = "yearly")]
        Yearly,
        [EnumMember(Value = "monthly")]
        Monthly,
        [EnumMember(Value = "daily")]
        Daily,
        [EnumMember(Value = "hourly")]
        Hourly,
        [EnumMember(Value = "minutely")]
        Minutely,
        [EnumMember(Value = "secondly")]
        Secondly
    }
    
    public class RecurringOptions
    {
        public FrequencyOptions Frequency { get; set; } = FrequencyOptions.Daily;
        public float Every { get; set; } = 1;
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? Until { get; set; }
        public int? Iterations { get; set; }
    }
}