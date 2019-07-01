using System;

namespace Nodester.Data.Models.Json_Models.Fields
{
    public abstract class BaseField
    {
        public Guid Key { get; set; }
        public string Label { get; set; }
    }
}