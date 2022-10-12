using System;
using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/n列置き/標準偏差")]
    public class StandardDeviationEveryN : ICalculationNode, IOptionNode
    {
        public int N { get; set; } = 1;
        
        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("n", new Value(N));
        }

        public void SetOptions(params Value[] options)
        {
            N = Math.Max(1, options[0].ToInt());
        }

        public Table Run(Table data)
        {
            var rows = new List<Value[]>();

            for (int rowIndex = 0; rowIndex < data.RowCount; ++rowIndex)
            {
                var row = data.GetRow(rowIndex).ToDoubleArray();

                var sumValues = new double[N];
                var counts = new int[N];
                for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
                {
                    var index = columnIndex % N;
                    sumValues[index] += row[columnIndex];
                    ++counts[index];
                }

                var averageValues = new double[N];
                for(int i = 0; i < N; ++i)
                {
                    averageValues[i] = sumValues[i] / counts[i];
                }

                var varianceValues = new double[N];
                for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
                {
                    var index = columnIndex % N;
                    var d = row[columnIndex] - averageValues[index];
                    varianceValues[index] += (d * d) / counts[index];
                }

                var results = new double[N];
                for (int i = 0; i < N; ++i)
                {
                    results[i] = Math.Sqrt(varianceValues[i]);
                }
                rows.Add(results.ToValueArray());
            }

            return Table.CreateFromRows(rows);
        }
    }
}
