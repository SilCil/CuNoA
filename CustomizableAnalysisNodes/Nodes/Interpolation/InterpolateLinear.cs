using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("補間/線形")]
    public class InterpolateLinear : ICalculationNode, IOptionNode
    {
        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;

        public double StartX { get; set; } = 0.0;
        public double EndX { get; set; } = 10.0;
        public double StepX { get; set; } = 1.0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("yの列番号", new Value(IndexY));
            yield return ("xの開始値", new Value(StartX));
            yield return ("xの終了値", new Value(EndX));
            yield return ("xの刻み", new Value(StepX));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToIntValue().IntValue;
            IndexY = options[1].ToIntValue().IntValue;
            StartX = options[2].ToDoubleValue().DoubleValue;
            EndX = options[3].ToDoubleValue().DoubleValue;
            StepX = options[4].ToDoubleValue().DoubleValue;
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).Select(x => x.ToDoubleValue().DoubleValue).ToArray();
            var dataY = data.GetColumn(IndexY).Select(x => x.ToDoubleValue().DoubleValue).ToArray();

            var originalDataX = new double[dataX.Length];
            Array.Copy(dataX, originalDataX, dataX.Length);
            Array.Sort(originalDataX, dataX);
            Array.Sort(originalDataX, dataY);

            var interpolation = Interpolate.Linear(dataX, dataY);

            var outX = new List<Value>();
            var outY = new List<Value>();
            for (double x = StartX; x <= EndX; x += StepX)
            {
                outX.Add(new Value(x));
                outY.Add(new Value(interpolation.Interpolate(x)));
            }

            return Table.CreateFromColumns(outX, outY);
        }
    }
}
