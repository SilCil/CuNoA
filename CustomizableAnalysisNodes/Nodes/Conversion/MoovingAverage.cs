using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/スムージング/単純移動平均")]
    public class MoovingAverage : IOptionNode, ICalculationNode
    {
        public int Index { get; set; } = 0;
        public int PointCount { get; set; } = 5;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
            yield return ("平均点数（奇数）", new Value(PointCount));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
            PointCount = (options[1].ToInt() / 2) * 2 + 1;
        }

        public Table Run(Table data)
        {
            var values = data.GetColumn(Index).ToDoubleArray();
            var smoothed = new double[values.Length];

            var n = PointCount / 2;
            for(int i = 0; i < values.Length; ++i)
            {
                var count = 0;
                var sum = 0.0;
                for(int j = i - n; j <= i + n; ++j)
                {
                    if (j < 0) continue;
                    if (j >= values.Length) continue;
                    sum += values[j];
                    ++count;
                }
                smoothed[i] = sum / count;
            }

            var columns = new List<IEnumerable<Value>>();
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                if (i == Index)
                {
                    columns.Add(smoothed.Select(x => new Value(x)));
                }
                else
                {
                    columns.Add(data.GetColumn(i));
                }
            }
            return Table.CreateFromColumns(columns);
        }
    }
}
