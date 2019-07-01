namespace Nodester.Graph.Core
{
    public class CachedValue
    {
        public bool HasValue => Value != default(object);
        public object Value { get; private set; }

        public CachedValue()
        {
        }

        public void SetValue(object value)
        {
            Value = value;
        }
    }
}