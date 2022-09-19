using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/数値/微分（3次のスプラインを使用）")]
    public class DifferentiateByCubicSpline : ICalculationNode, IOptionNode
    {
        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("yの列番号", new Value(IndexY));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToIntValue().IntValue;
            IndexY = options[1].ToIntValue().IntValue;
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).Select(x => x.ToDoubleValue().DoubleValue).ToArray();
            var dataY = data.GetColumn(IndexY).Select(x => x.ToDoubleValue().DoubleValue).ToArray();

            var originalDataX = new double[dataX.Length];
            Array.Copy(dataX, originalDataX, dataX.Length);
            Array.Sort(originalDataX, dataX);
            Array.Sort(originalDataX, dataY);

            var interpolation = Interpolate.CubicSpline(dataX, dataY);

            var rows = new List<IEnumerable<Value>>();
            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                row[IndexY] = new Value(interpolation.Differentiate(row[IndexX].ToDoubleValue().DoubleValue));
                rows.Add(row);
            }

            return Table.CreateFromRows(rows);
        }
    }
}
