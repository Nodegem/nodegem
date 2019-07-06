using System.Runtime.Serialization;

namespace Nodester.Engine.Data
{
    public enum ValueType
    {
        [EnumMember(Value = "any")]
        Any = 0,
        [EnumMember(Value = "color")]
        Color = 1,
        [EnumMember(Value = "time")]
        Time = 2,
        [EnumMember(Value = "date")]
        Date = 3,
        [EnumMember(Value = "datetime")]
        DateTime = 4,
        [EnumMember(Value = "boolean")]
        Boolean = 5,
        [EnumMember(Value = "text")]
        Text = 6,
        [EnumMember(Value = "textarea")]
        TextArea = 7,
        [EnumMember(Value = "url")]
        Url = 8,
        [EnumMember(Value = "phonenumber")]
        PhoneNumber = 9,
        [EnumMember(Value = "number")]
        Number = 10
    }
}