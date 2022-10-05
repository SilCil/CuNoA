using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/複数列を1列に/平均")]
    public class AverageColumns : ICalculationNode, IOptionNode
    {
        public int[] Indices { get; set; } = new int[] { 0, 1 };

        public IEnumerable<(string label, Value)> GetOptions()
        {
            var count = new Value(Indices is null ? 0 : Indices.Length);
            yield return ("列の数", count);

            for (int i = 0; i < count.IntValue; ++i)
            {
                yield return ($"列番号{i}", new Value(Indices[i]));
            }
        }

        public void SetOptions(params Value[] options)
        {
            if (options is null || options.Length == 0)
            {
                Indices = new int[0];
            }
            else
            {
                Indices = new int[GetValue(options[0])];
            }

            for (int i = 0; i < Indices.Length; ++i)
            {
                Indices[i] = (i + 1 < options.Length) ? GetValue(options[i + 1]) : GetValue(options.Last());
            }
        }

        private int GetValue(Value value)
        {
            var v = value.ToIntValue().IntValue;
            return (v < 0) ? 0 : v;
        }

        public Table Run(Table data)
        {
            var outputColumns = new List<IReadOnlyList<Value>>();
            var targetColumns = new List<double[]>();
            int insertIndex = -1;

            for(int i = 0; i < data.ColumnCount; ++i)
            {
                var column = data.GetColumn(i);

                if(Indices.Contains(i))
                {
                    targetColumns.Add(column.ToDoubleArray());
                    if (insertIndex < 0)
                    {
                        insertIndex = outputColumns.Count;
                    }
                }
                else
                {
                    outputColumns.Add(column);
                }
            }

            if (targetColumns.Count != 0)
            {
                var averages = new double[targetColumns[0].Length];
                
                for(int row = 0; row < averages.Length; ++row)
                {
                    averages[row] = 0.0;

                    for(int column = 0; column < targetColumns.Count; ++column)
                    {
                        averages[row] += targetColumns[column][row];
                    }

                    averages[row] /= targetColumns.Count;
                }

                outputColumns.Insert(insertIndex, averages.ToValueArray());
            }

            return Table.CreateFromColumns(outputColumns);
        }
    }
}
