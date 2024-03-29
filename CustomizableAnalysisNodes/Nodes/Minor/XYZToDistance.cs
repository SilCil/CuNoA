﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using CustomizableAnalysisLibrary;
using MathNet.Numerics.LinearAlgebra;

namespace Kato.EvAX
{
    [Node("専門/EvAX/距離初期値, 平均距離, 標準偏差を計算")]
    public class XYZToDistance : ICalculationNode, IOptionNode
    {
        public string Absorber { get; set; } = "Ba";
        public double MinDistance { get; set; } = 0.1;
        public double MaxDistance { get; set; } = 4.2;
        public double DistanceEpsilon { get; set; } = 0.001;
        public bool GroupBySite { get; set; } = true;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("吸収原子", new Value(Absorber));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("Group閾値", new Value(DistanceEpsilon));
            yield return ("Siteを区別する", new Value(GroupBySite));
        }

        public void SetOptions(params Value[] options)
        {
            Absorber = options[0].ToString();
            MinDistance = options[1].ToDouble();
            MaxDistance = options[2].ToDouble();
            DistanceEpsilon = options[3].ToDouble();
            GroupBySite = options[4].ToBool();
        }

        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);
            
            CalculateDistances(initialAllPositions, finalLabels, finalRealPositions, finalAllPositions, out var bondLabels, out var initialDistances, out var distances);

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

        public void CalculateDistances(List<Vector<double>> initialAllPositions, List<string> finalLabels, List<Vector<double>> finalRealPositions, List<Vector<double>> finalAllPositions, out List<string> bondLabels, out List<double> initialDistances, out List<List<double>> distances)
        {
            bondLabels = new List<string>();
            initialDistances = new List<double>();
            distances = new List<List<double>>();
            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                if (finalLabels[i] != Absorber) continue;

                for (int j = 0; j < finalAllPositions.Count; ++j)
                {
                    var initialDistance = Distance.Euclidean(initialAllPositions[i], initialAllPositions[j]);

                    if (initialDistance < MinDistance) continue;
                    if (initialDistance > MaxDistance) continue;

                    var bondLabel = GetBondLabel(finalLabels[i], finalLabels[j]);
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
        }

        private string GetBondLabel(string label1, string label2)
        {
            if (GroupBySite)
            {
                return string.Join("-", label1, label2);
            }
            else
            {
                return string.Join("-", RemoveNumber(label1), RemoveNumber(label2));
            }
        }

        private string RemoveNumber(string str)
        {
            return new string(str.Where(x => char.IsDigit(x) == false).ToArray());
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
