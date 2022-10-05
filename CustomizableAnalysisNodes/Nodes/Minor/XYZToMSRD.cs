using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using CustomizableAnalysisLibrary;

namespace Kato.EvAX
{
    [Node("専門/EvAX/MSRD, MSRD（||）, MSRD（⊥）, 2<ui.uj>を計算")]
    public class XYZToMSRD : ICalculationNode, IOptionNode
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
            yield return ("距離の誤差", new Value(DistanceEpsilon));
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

            var displacementsFromInitial = XYZToOffset.CalcDisplacements(finalLabels, initialRealPositions, finalRealPositions);
            var averageDisplacementsFromInitial = XYZToOffset.CalcAverageDisplacements(displacementsFromInitial);

            var bondLabels = new List<string>();
            var initialDistances = new List<double>();

            var msrdValues = new List<List<double>>();
            var msrdParallelValues = new List<List<double>>();
            var msrdPerpendicularValues = new List<List<double>>();
            var correlationValues = new List<List<double>>();

            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                var label_i = finalLabels[i];
                if (label_i != Absorber) continue;

                var r0_i = initialRealPositions[i] + averageDisplacementsFromInitial[label_i];
                var u_i = finalRealPositions[i] - r0_i;

                for (int j = 0; j < finalAllPositions.Count; ++j)
                {
                    var initialDistance = Distance.Euclidean(initialAllPositions[i], initialAllPositions[j]);
                    if (initialDistance < MinDistance) continue;
                    if (initialDistance > MaxDistance) continue;

                    var label_j = finalLabels[j];
                    var r0_j = initialAllPositions[j] + averageDisplacementsFromInitial[label_j];
                    var u_j = finalAllPositions[j] - r0_j;

                    var bondLabel = $"{label_i}-{label_j}";

                    var r0 = r0_j - r0_i;
                    var r0_direction = r0 / r0.L2Norm();
                    var u = u_j - u_i;
                    var u2 = u.DotProduct(u);
                    var u2_parallel = Math.Pow(r0_direction.DotProduct(u), 2);
                    var u2_perpendicular = u2 - u2_parallel;
                    var correlation = 2.0 * u_i.DotProduct(u_j);

                    var index = GetIndex(bondLabels, initialDistances, initialDistance, bondLabel);
                    var insertIndex = -1;

                    if (index < 0)
                    {
                        insertIndex = initialDistances.Count(x => x <= initialDistance);

                        bondLabels.Insert(insertIndex, bondLabel);
                        initialDistances.Insert(insertIndex, initialDistance);
                    }

                    AddValue(ref msrdValues, index, insertIndex, u2);
                    AddValue(ref msrdParallelValues, index, insertIndex, u2_parallel);
                    AddValue(ref msrdPerpendicularValues, index, insertIndex, u2_perpendicular);
                    AddValue(ref correlationValues, index, insertIndex, correlation);
                }
            }

            var rows = new List<Value[]>();
            for (int i = 0; i < bondLabels.Count; ++i)
            {
                rows.Add(new Value[]
                {
                    new Value(bondLabels[i]),
                    new Value(msrdValues[i].Average()),
                    new Value(msrdParallelValues[i].Average()),
                    new Value(msrdPerpendicularValues[i].Average()),
                    new Value(correlationValues[i].Average()),
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

        private void AddValue(ref List<List<double>> target, int index, int insertIndex, double value)
        {
            if (index >= 0)
            {
                target[index].Add(value);
            }
            else
            {
                target.Insert(insertIndex, new List<double>() { value });
            }
        }
    }
}
