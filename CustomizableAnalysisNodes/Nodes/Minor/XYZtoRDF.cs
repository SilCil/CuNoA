using System.Linq;
using System.Collections.Generic;
using CustomizableAnalysisLibrary;
using CustomizableAnalysisLibrary.Nodes;

namespace Kato.EvAX
{
    [Node("専門/EvAX/RDFを計算")]
    public class XYZtoRDF : ICalculationNode, IOptionNode
    {
        public string Absorber { get; set; } = "Ba";
        public double MinDistance { get; set; } = 0.005;
        public double MaxDistance { get; set; } = 8.0;
        public double DeltaR { get; set; } = 0.01;
        public double DistanceEpsilon { get; set; } = 0.001;
        public bool GroupBySite { get; set; } = true;

        private XYZToDistance m_distanceCalculator = new XYZToDistance();
        private MakeHistogram m_histogramCalculator = new MakeHistogram();

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("吸収原子", new Value(Absorber));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("bin幅", new Value(DeltaR));
            yield return ("Group閾値", new Value(DistanceEpsilon));
            yield return ("Siteを区別する", new Value(GroupBySite));
        }

        public void SetOptions(params Value[] options)
        {
            Absorber = options[0].ToString();
            MinDistance = options[1].ToDouble();
            MaxDistance = options[2].ToDouble();
            DeltaR = options[3].ToDouble();
            DistanceEpsilon = options[4].ToDouble();
            GroupBySite = options[5].ToBool();

            ApplyOptions();
        }

        private void ApplyOptions()
        {
            m_distanceCalculator.Absorber = Absorber;
            m_distanceCalculator.MinDistance = MinDistance;
            m_distanceCalculator.MaxDistance = MaxDistance;
            m_distanceCalculator.DistanceEpsilon = DistanceEpsilon;
            m_distanceCalculator.GroupBySite = GroupBySite;

            m_histogramCalculator.MinValue = MinDistance;
            m_histogramCalculator.MaxValue = MaxDistance;
            m_histogramCalculator.Delta = DeltaR;
        }

        public Table Run(Table data)
        {
            ApplyOptions();

            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            m_distanceCalculator.CalculateDistances(initialAllPositions, finalLabels, finalRealPositions, finalAllPositions, out var bondLabels, out var initialDistances, out var distances);
            m_histogramCalculator.CalculateXValues(out var rValues);

            int absorberCount = 0;
            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                if (finalLabels[i] == Absorber)
                {
                    ++absorberCount;
                }
            }

            var rdfValues = new double[bondLabels.Count][];
            for(int i = 0; i < rdfValues.Length; ++i)
            {
                m_histogramCalculator.CalculateBinValues(distances[i], out rdfValues[i]);

                for(int j = 0; j < rdfValues[i].Length; ++j)
                {
                    rdfValues[i][j] /= DeltaR;
                    rdfValues[i][j] /= absorberCount;
                }
            }

            var columns = new List<IEnumerable<Value>>();
            columns.Add(rValues.ToValueArray().Prepend(new Value("#r")));
            for(int i = 0; i < bondLabels.Count; ++i)
            {
                columns.Add(rdfValues[i].ToValueArray().Prepend(new Value(bondLabels[i])));
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
