using System;

namespace CustomizableAnalysisLibrary
{
    public class Value : IEquatable<Value>
    {
        public ValueType ValueType { get; private set; } = default;
        public int IntValue { get; private set; } = default;
        public string StringValue { get; private set; } = default;
        public double DoubleValue { get; private set; } = default;
        public bool BoolValue { get; private set; } = default;

        public Value(int value)
        {
            ValueType = ValueType.Int;
            IntValue = value;
        }

        public Value(string value)
        {
            ValueType = ValueType.String;
            StringValue = value ?? string.Empty;
        }

        public Value(double value)
        {
            ValueType = ValueType.Double;
            DoubleValue = value;
        }

        public Value(bool value)
        {
            ValueType = ValueType.Bool;
            BoolValue = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null)) return false;

            return (obj is Value other) && Equals(other);
        }

        public bool Equals(Value other)
        {
            if (ValueType != other.ValueType) return false;

            return ValueType switch
            {
                ValueType.Int => IntValue.Equals(other.IntValue),
                ValueType.String => StringValue.Equals(other.StringValue),
                ValueType.Double => DoubleValue.Equals(other.DoubleValue),
                ValueType.Bool => BoolValue.Equals(other.BoolValue),
                _ => throw new NotImplementedException(),
            };
        }

        public override int GetHashCode()
        {
            return ValueType switch
            {
                ValueType.Int => IntValue.GetHashCode(),
                ValueType.String => StringValue.GetHashCode(),
                ValueType.Double => DoubleValue.GetHashCode(),
                ValueType.Bool => BoolValue.GetHashCode(),
                _ => throw new NotImplementedException(),
            };
        }

        public static bool operator ==(Value x, Value y) => x.Equals(y);
        public static bool operator !=(Value x, Value y) => !(x == y);

        public override string ToString()
        {
            return ValueType switch
            {
                ValueType.Int => IntValue.ToString(),
                ValueType.String => StringValue.ToString(),
                ValueType.Double => DoubleValue.ToString(),
                ValueType.Bool => BoolValue.ToString(),
                _ => throw new NotImplementedException(),
            };
        }

        public static Value ToStringValue(Value element) => new Value(element.ToString());
        public Value ToStringValue() => ToStringValue(this);

        public static Value ToIntValue(Value element)
        {
            return element.ValueType switch
            {
                ValueType.Int => element,
                ValueType.String => new Value(int.Parse(element.StringValue)),
                ValueType.Double => new Value((int)element.DoubleValue),
                ValueType.Bool => new Value(element.BoolValue ? 1 : 0),
                _ => throw new NotImplementedException(),
            };
        }
        public Value ToIntValue() => ToIntValue(this);

        public static Value ToDoubleValue(Value element)
        {
            return element.ValueType switch
            {
                ValueType.Int => new Value((double)element.IntValue),
                ValueType.String => new Value(double.TryParse(element.StringValue, out var v) ? v : double.NaN),
                ValueType.Double => element,
                ValueType.Bool => new Value(element.BoolValue ? 1.0 : 0.0),
                _ => throw new NotImplementedException(),
            };
        }
        public Value ToDoubleValue() => ToDoubleValue(this);

        public static Value ToBoolValue(Value element)
        {

            return element.ValueType switch
            {
                ValueType.Int => new Value(element.IntValue != 0),
                ValueType.String => new Value(bool.Parse(element.StringValue)),
                ValueType.Double => new Value(element.DoubleValue != 0),
                ValueType.Bool => element,
                _ => throw new NotImplementedException(),
            };
        }
        public Value ToBoolValue() => ToBoolValue(this);
    }
}
