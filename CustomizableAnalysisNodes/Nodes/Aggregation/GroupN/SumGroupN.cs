using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/n列で区切る/合計")]
    public class SumGroupN : ICalculationNode, IOptionNode
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

                var outputValues = new List<double>();

                for (int columnIndex = 0; columnIndex < row.Length;)
                {
                    var values = new List<double>();
                    for (int i = 0; i < N; ++i)
                    {
                        if (columnIndex >= row.Length) break;

                        values.Add(row[columnIndex]);
                        ++columnIndex;
                    }

                    var average = values.Average();
                    outputValues.Add(average);
                }

                rows.Add(outputValues.ToValueArray());
            }

            return Table.CreateFromRows(rows);
        }
    }
}
