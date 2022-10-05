using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary
{
    public static class ValueExtensions
    {
        public static double[] ToDoubleArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToDoubleValue().DoubleValue).ToArray();
        }

        public static int[] ToIntArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToIntValue().IntValue).ToArray();
        }

        public static string[] ToStringArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToStringValue().StringValue).ToArray();
        }

        public static Value[] ToValueArray(this IEnumerable<double> values)
        {
            return values.Select(x => new Value(x)).ToArray();
        }
    }
}
