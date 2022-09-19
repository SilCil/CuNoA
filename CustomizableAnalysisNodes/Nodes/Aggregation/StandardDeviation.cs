using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/標準偏差")]
    public class StandardDeviation : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToIntValue().IntValue;
        }

        public Table Run(Table data)
        {
            var column = data.GetColumn(Index).Select(x => x.ToDoubleValue().DoubleValue).ToArray();
            var average = column.Average();
            var sigma2 = column.Average(x => (x - average) * (x - average));
            var std = Math.Sqrt(sigma2);
            return Table.CreateFromSingleElement(new Value(std));
        }
    }
}
