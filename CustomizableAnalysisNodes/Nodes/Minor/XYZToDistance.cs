using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using CustomizableAnalysisLibrary;

namespace Kato.EvAX
{
    [Node("専門/EvAX/距離初期値, 平均距離, 標準偏差を計算")]
    public class XYZToDistance : ICalculationNode, IOptionNode
    {
        public string Absorber { get; set; } = "Ba";
        public double MinDistance { get; set; } = 0.1;
        public double MaxDistance { get; set; } = 4.2;
        public double DistanceEpsilon { get; set; } = 0.001;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("吸収原子", new Value(Absorber));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("Group閾値", new Value(DistanceEpsilon));
        }

        public void SetOptions(params Value[] options)
        {
            Absorber = options[0].ToStringValue().StringValue;
            MinDistance = options[1].ToDoubleValue().DoubleValue;
            MaxDistance = options[2].ToDoubleValue().DoubleValue;
            DistanceEpsilon = options[3].ToDoubleValue().DoubleValue;
        }

        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            var bondLabels = new List<string>();
            var initialDistances = new List<double>();
            var distances = new List<List<double>>();

            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                if (finalLabels[i] != Absorber) continue;

                for (int j = 0; j < finalAllPositions.Count; ++j)
                {
                    var initialDistance = Distance.Euclidean(initialAllPositions[i], initialAllPositions[j]);

                    if (initialDistance < MinDistance) continue;
                    if (initialDistance > MaxDistance) continue;

                    var bondLabel = $"{finalLabels[i]}-{finalLabels[j]}";
                    var distance = Distance.Euclidean(finalAllPositions[i], finalAllPositions[j]);

                    var index = GetIndex(bondLabels, initialDistances, initialDistance, bondLabel);
                    
                    if (index < 0)
                    {
                        index = initialDistances.Count(x => x <= initialDistance);

                        bondLabels.Insert(index, bondLabel);
                        initialDistances.Insert(index, initialDistance);
                        distances.Insert(index, new List<double>());
                    }

                    distances[index].Add(distance);
                }
            }

            var rows = new List<Value[]>();
            for (int i = 0; i < bondLabels.Count; ++i)
            {
                rows.Add(new Value[]
                {
                    new Value(bondLabels[i]),
                    new Value(initialDistances[i]),
                    new Value(distances[i].Average()),
                    new Value(distances[i].StandardDeviation()),
                });
            }
            return Table.CreateFromRows(rows);
        }

        private int GetIndex(List<string> labels, List<double> initialDistances, double initialDistance, string label)
        {
            for (int i = 0; i < labels.Count; ++i)
            {
                if (labels[i] != label) continue;
                if (Math.Abs(initialDistances[i] - initialDistance) > DistanceEpsilon) continue;
                return i;
            }
            return -1;
        }
    }
}
