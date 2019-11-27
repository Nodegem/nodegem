using System.Runtime.Serialization;

namespace Nodegem.Common.Data
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
}