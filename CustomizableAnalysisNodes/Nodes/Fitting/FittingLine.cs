using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("フィッティング/直線")]
    public class FittingLine : ICalculationNode, IOptionNode
    {
        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool OutputCurve { get; set; } = true;

        public double Intercept { get; private set; } = default;
        public double Slope { get; private set; } = default;

        private double EvaluateFunc(double x) => Intercept + Slope * x;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("yの列番号", new Value(IndexY));
            yield return ("x, y, f(x)を出力（Noの場合、切片と傾きを出力）", new Value(OutputCurve));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToIntValue().IntValue;
            IndexY = options[1].ToIntValue().IntValue;
            OutputCurve = options[2].ToBoolValue().BoolValue;
        }

        public Table Run(Table data)
        {
            var dataX = data.GetColumn(IndexX).Select(x => x.ToDoubleValue().DoubleValue).ToArray();
            var dataY = data.GetColumn(IndexY).Select(y => y.ToDoubleValue().DoubleValue).ToArray();
            (Intercept, Slope) = Fit.Line(dataX, dataY);

            if(OutputCurve)
            {
                return Table.CreateFromColumns(data.GetColumn(IndexX), data.GetColumn(IndexY), dataX.Select(x => new Value(EvaluateFunc(x))));
            }
            else
            {
                return Table.CreateFromRows(new Value[] { new Value(Intercept), new Value(Slope) });
            }
        }
    }
}
