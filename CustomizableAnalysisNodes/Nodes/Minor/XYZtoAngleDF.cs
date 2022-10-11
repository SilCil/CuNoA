using System.Collections.Generic;
using CustomizableAnalysisLibrary;
using CustomizableAnalysisLibrary.Nodes;

namespace Kato.EvAX
{
    [Node("専門/EvAX/AngleDFを計算")]
    public class XYZtoAngleDF : ICalculationNode, IOptionNode
    {
        public string[] Atoms { get; set; } = new string[] { "Ba", "Ti", "Ba" };

        public double MinDistance { get; set; } = 0.1;
        public double MaxDistance { get; set; } = 6.0;
        public double DistanceEpsilon { get; set; } = 0.001;

        public double MinAngle { get; set; } = 0.0;
        public double MaxAngle { get; set; } = 180.0;
        public double Delta { get; set; } = 0.1;
        public double AngleDegEpsilon { get; set; } = 0.1;

        private XYZToAngle m_angleCalculator = new XYZToAngle();
        private MakeHistogram m_histogramCalculator = new MakeHistogram();

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("原子1", new Value(Atoms[0]));
            yield return ("原子2", new Value(Atoms[1]));
            yield return ("原子3", new Value(Atoms[2]));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("Group閾値（距離）", new Value(DistanceEpsilon));
            yield return ("Group閾値（角度）", new Value(AngleDegEpsilon));
            yield return ("最小角度", new Value(MinAngle));
            yield return ("最大角度", new Value(MaxAngle));
            yield return ("bin幅", new Value(Delta));
        }

        public void SetOptions(params Value[] options)
        {
            Atoms = new string[]
            {
                options[0].ToString(),
                options[1].ToString(),
                options[2].ToString()
            };
            MinDistance = options[3].ToDouble();
            MaxDistance = options[4].ToDouble();
            DistanceEpsilon = options[5].ToDouble();
            AngleDegEpsilon = options[6].ToDouble();
            MinAngle = options[7].ToDouble();
            MaxAngle = options[8].ToDouble();
            Delta = options[9].ToDouble();

            ApplyOptions();
        }

        private void ApplyOptions()
        {
            m_angleCalculator.Atoms = Atoms;
            m_angleCalculator.MinDistance = MinDistance;
            m_angleCalculator.MaxDistance = MaxDistance;
            m_angleCalculator.DistanceEpsilon = DistanceEpsilon;
            m_angleCalculator.AngleDegEpsilon = AngleDegEpsilon;

            m_histogramCalculator.MinValue = MinAngle;
            m_histogramCalculator.MaxValue = MaxAngle;
            m_histogramCalculator.Delta = Delta;
        }

        public Table Run(Table data)
        {
            ApplyOptions();

            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            m_histogramCalculator.CalculateXValues(out var xValues);

            m_angleCalculator.CalculateAngles(initialRealPositions, initialAllPositions, finalLabels, finalAllPositions, out var initialDistances, out var initialAngles, out var angles);

            int atomCount = 0;
            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                if (finalLabels[i] == Atoms[2])
                {
                    ++atomCount;
                }
            }

            var adfValues = new double[angles.Count][];
            for(int i = 0; i < adfValues.Length; ++i)
            {
                m_histogramCalculator.CalculateBinValues(angles[i], out adfValues[i]);

                for(int j = 0; j < adfValues[i].Length; ++j)
                {
                    adfValues[i][j] /= Delta;
                    adfValues[i][j] /= atomCount;
                }
            }

            var columns = new List<IEnumerable<Value>>();
            columns.Add(xValues.ToValueArray());
            for(int i = 0; i < adfValues.Length; ++i)
            {
                columns.Add(adfValues[i].ToValueArray());
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
