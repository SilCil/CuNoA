using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/列/標準偏差")]
    public class StandardDeviation : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            var column = data.GetColumn(Index).ToDoubleArray();
            var average = column.Average();

            var sigma2 = 0.0;
            for(int i = 0;i < column.Length; ++i)
            {
                var value = column[i];
                sigma2 += (value - average) * (value - average);
            }
            sigma2 /= column.Length;

            var std = Math.Sqrt(sigma2);
            return Table.CreateFromSingleElement(new Value(std));
        }
    }
}
