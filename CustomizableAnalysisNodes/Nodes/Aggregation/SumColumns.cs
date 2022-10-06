using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/複数列を1列に/合計")]
    public class SumColumns : ICalculationNode, IOptionNode
    {
        public bool IsRemoveColumns { get; set; } = true;
        public int[] Indices { get; set; } = new int[] { 0, 1 };

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("用いた列を削除する", new Value(IsRemoveColumns));

            var count = new Value(Indices is null ? 0 : Indices.Length);
            yield return ("列の数", count);

            for (int i = 0; i < count.IntValue; ++i)
            {
                yield return ($"列番号{i}", new Value(Indices[i]));
            }
        }

        public void SetOptions(params Value[] options)
        {
            IsRemoveColumns = options[0].ToBool();

            var indicesCount = (options.Length <= 1) ? 0 : Math.Max(0, options[1].ToInt());
            Indices = new int[indicesCount];

            int indexOffset = 2;
            for (int i = 0; i < Indices.Length; ++i)
            {
                if (i + indexOffset < options.Length)
                {
                    Indices[i] = Math.Max(0, options[i + indexOffset].ToInt());
                }
                else
                {
                    Indices[i] = Math.Max(0, Indices[i - 1]);
                }
            }
        }

        public Table Run(Table data)
        {
            var outputColumns = new List<IReadOnlyList<Value>>();
            var targetColumns = new List<double[]>();

            for (int i = 0; i < data.ColumnCount; ++i)
            {
                var column = data.GetColumn(i);

                if (Indices.Contains(i))
                {
                    if (IsRemoveColumns == false)
                    {
                        outputColumns.Add(column);
                    }

                    targetColumns.Add(column.ToDoubleArray());
                }
                else
                {
                    outputColumns.Add(column);
                }
            }

            if (targetColumns.Count != 0)
            {
                var sumValues = new double[targetColumns[0].Length];

                for (int row = 0; row < sumValues.Length; ++row)
                {
                    sumValues[row] = 0.0;

                    for (int column = 0; column < targetColumns.Count; ++column)
                    {
                        sumValues[row] += targetColumns[column][row];
                    }

                    sumValues[row] /= targetColumns.Count;
                }

                outputColumns.Add(sumValues.ToValueArray());
            }

            return Table.CreateFromColumns(outputColumns);
        }
    }
}
