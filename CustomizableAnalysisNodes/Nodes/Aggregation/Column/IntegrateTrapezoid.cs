using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/列/積分（台形積分）")]
    public class IntegrateTrapezoid : ICalculationNode, IOptionNode
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
            IndexX = options[0].ToInt();
            IndexY = options[1].ToInt();
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).ToDoubleArray();
            var dataY = data.GetColumn(IndexY).ToDoubleArray();
            
            var originalDataX = new double[dataX.Length];
            Array.Copy(dataX, originalDataX, dataX.Length);
            Array.Sort(originalDataX, dataX);
            Array.Sort(originalDataX, dataY);

            double sum = 0.0;
            for(int i = 1; i < dataX.Length; ++i)
            {
                sum += 0.5 * (dataY[i] + dataY[i - 1]) * (dataX[i] - dataX[i - 1]);
            }

            return Table.CreateFromSingleElement(new Value(sum));
        }
    }
}
