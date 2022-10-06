using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary
{
    public static class ValueExtensions
    {
        public static double[] ToDoubleArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToDouble()).ToArray();
        }

        public static int[] ToIntArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToInt()).ToArray();
        }

        public static string[] ToStringArray(this IEnumerable<Value> values)
        {
            return values.Select(x => x.ToString()).ToArray();
        }

        public static Value[] ToValueArray(this IEnumerable<double> values)
        {
            return values.Select(x => new Value(x)).ToArray();
        }

        public static Value[] ToValueArray(this IEnumerable<string> values)
        {
            return values.Select(x => new Value(x)).ToArray();
        }
    }
}
