using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/列/ヒストグラム")]
    public class MakeHistogram : IOptionNode, ICalculationNode
    {
        public int Index { get; set; } = 0;
        public double MinValue { get; set; } = 0.01;
        public double MaxValue { get; set; } = 10.0;
        public double Delta { get; set; } = 0.1;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
            yield return ("最小", new Value(MinValue));
            yield return ("最大", new Value(MaxValue));
            yield return ("bin幅", new Value(Delta));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
            MinValue = options[1].ToDouble();
            MaxValue = options[2].ToDouble();
            Delta = options[3].ToDouble();
        }

        public Table Run(Table data)
        {
            CalculateXValues(out var xValues);

            var targetValues = data.GetColumn(Index).ToDoubleArray();
            CalculateBinValues(targetValues, out var yValues);

            var columns = new Value[][] 
            {
                xValues.ToValueArray(),
                yValues.ToValueArray(),
            };

            return Table.CreateFromColumns(columns);
        }

        public void CalculateXValues(out double[] result)
        {
            result = CreateEmptyArray();
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = MinValue + Delta * i;
            }
        }

        public void CalculateBinValues(IEnumerable<double> targetValues, out double[] result)
        {
            result = CreateEmptyArray();

            foreach (var value in targetValues)
            {
                if (value > MaxValue) continue;
                if (value < MinValue) continue;

                var binIndex = GetBinIndex(value);
                result[binIndex] += 1.0;
            }
        }

        private double[] CreateEmptyArray()
        {
            int binCount = GetBinIndex(MaxValue) + 1;
            var values = new double[binCount];
            return values;
        }

        private int GetBinIndex(double value)
        {
            // 四捨五入のため, +0.5
            return (int)((value - MinValue) / Delta + 0.5);
        }
    }
}
