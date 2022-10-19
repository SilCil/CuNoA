using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("補間/3次のスプライン")]
    public class InterpolateCubicSpline : ICalculationNode, IOptionNode
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
            IndexX = options[0].ToInt();
            IndexY = options[1].ToInt();
            StartX = options[2].ToDouble();
            EndX = options[3].ToDouble();
            StepX = options[4].ToDouble();
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).ToDoubleArray();
            var dataY = data.GetColumn(IndexY).ToDoubleArray();

            var originalDataX = new double[dataX.Length];
            Array.Copy(dataX, originalDataX, dataX.Length);
            Array.Sort(originalDataX, dataX);
            Array.Sort(originalDataX, dataY);

            var interpolation = Interpolate.CubicSpline(dataX, dataY);

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
