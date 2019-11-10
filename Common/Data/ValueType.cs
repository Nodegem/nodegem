using System.Runtime.Serialization;

namespace Nodester.Common.Data
{
    public enum ValueType
    {
        [EnumMember(Value = "any")]
        Any = 0,
        [EnumMember(Value = "time")]
        Time = 1,
        [EnumMember(Value = "date")]
        Date = 2,
        [EnumMember(Value = "datetime")]
        DateTime = 3,
        [EnumMember(Value = "boolean")]
        Boolean = 4,
        [EnumMember(Value = "text")]
        Text = 5,
        [EnumMember(Value = "textarea")]
        TextArea = 6,
        [EnumMember(Value = "url")]
        Url = 7,
        [EnumMember(Value = "phonenumber")]
        PhoneNumber = 8,
        [EnumMember(Value = "number")]
        Number = 9
    }
}