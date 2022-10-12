using System;
using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/n列おき/合計")]
    public class SumEveryN : ICalculationNode, IOptionNode
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
            var sumValues = new double[N][];
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                var index = i % N;
                var column = data.GetColumn(i).ToDoubleArray();

                if (sumValues[index] is null)
                {
                    sumValues[index] = new double[column.Length];
                }

                for (int j = 0; j < column.Length; ++j)
                {
                    sumValues[index][j] += column[j];
                }
            }

            var columns = new Value[N][];
            for(int i = 0;i < sumValues.Length; ++i)
            {
                columns[i] = sumValues[i].ToValueArray();
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
