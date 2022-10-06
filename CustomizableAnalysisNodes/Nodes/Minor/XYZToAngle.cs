﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.LinearAlgebra;
using CustomizableAnalysisLibrary;

namespace Kato.EvAX
{
    [Node("専門/EvAX/角度初期値, 平均角度, 標準偏差, 距離初期値を計算")]
    public class XYZToAngle : ICalculationNode, IOptionNode
    {
        public string[] Atoms { get; set; } = new string[] { "Ba", "Ti", "Ba" };
        public double MinDistance { get; set; } = 0.1;
        public double MaxDistance { get; set; } = 6.0;
        public double DistanceEpsilon { get; set; } = 0.001;
        public double AngleDegEpsilon { get; set; } = 0.1;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("原子1", new Value(Atoms[0]));
            yield return ("原子2", new Value(Atoms[1]));
            yield return ("原子3", new Value(Atoms[2]));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("Group閾値（距離）", new Value(DistanceEpsilon));
            yield return ("Group閾値（角度）", new Value(AngleDegEpsilon));
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
        }

        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            var initialDistances = new List<double>();
            var initialAngles = new List<double>();
            var angles = new List<List<double>>();

            // Atom1 (Real) -> Atom2 (All) -> Atom3 (All)で全探索するとめっちゃ時間かかる.
            // ループカウント: RealCount*AllCount*AllCount = 3^6*RealCount^3
            // Atom2からスタートして検索量を減らす.

            // Atom2から一定距離内にあるものだけ扱うためのリスト.
            // (index, initialDistance)
            var targetAtoms = new List<(int, double)>();

            for (int j = 0; j < initialRealPositions.Count; ++j)
            {
                if (finalLabels[j] != Atoms[1]) continue;

                // ループカウント: RealCount*AllCount = 3^3*RealCount^2.
                // MaxDistanceが現実的な値なら要素数は100以下になるはず.
                // その場合、ループ全体として 3^3*RealCount^2 + RealCount*100*100.
                targetAtoms.Clear();
                for(int i = 0; i < initialAllPositions.Count; ++i)
                {
                    if (i == j) continue;
                    if (finalLabels[i] != Atoms[0] && finalLabels[i] != Atoms[2]) continue;

                    var initialDistance = Distance.Euclidean(initialAllPositions[i], initialAllPositions[j]);
                    if (initialDistance > MaxDistance * 2) continue;

                    targetAtoms.Add((i, initialDistance));
                }

                foreach ((var i, var initialDistance12) in targetAtoms)
                {
                    if (finalLabels[i] != Atoms[0]) continue;

                    foreach ((var k, var initialDistance23) in targetAtoms)
                    {
                        if (i == k) continue;
                        if (finalLabels[k] != Atoms[2]) continue;

                        var initialDistance31 = Distance.Euclidean(initialAllPositions[i], initialAllPositions[k]);
                        var initialDistance = (initialDistance12 + initialDistance23 + initialDistance31) / 2.0;

                        if (initialDistance < MinDistance) continue;
                        if (initialDistance > MaxDistance) continue;

                        var initialAngle = GetAngle(initialAllPositions, i, j, k);
                        var angle = GetAngle(finalAllPositions, i, j, k);

                        var index = GetIndex(initialDistances, initialDistance, initialAngles, initialAngle);

                        if (index < 0)
                        {
                            index = initialDistances.Count(x => x <= initialDistance);

                            initialDistances.Insert(index, initialDistance);
                            initialAngles.Insert(index, initialAngle);
                            angles.Insert(index, new List<double>());
                        }

                        angles[index].Add(angle);
                    }
                }
            }

            var rows = new List<Value[]>();
            for (int i = 0; i < initialDistances.Count; ++i)
            {
                rows.Add(new Value[]
                {
                    new Value(initialAngles[i]),
                    new Value(angles[i].Average()),
                    new Value(angles[i].StandardDeviation()),
                    new Value(initialDistances[i]),
                });
            }
            return Table.CreateFromRows(rows);
        }

        private int GetIndex(List<double> initialDistances, double initialDistance, List<double> initialAngles, double initialAngle)
        {
            for (int i = 0; i < initialDistances.Count; ++i)
            {
                if (Math.Abs(initialAngles[i] - initialAngle) > AngleDegEpsilon) continue;
                if (Math.Abs(initialDistances[i] - initialDistance) > DistanceEpsilon) continue;

                return i;
            }
            return -1;
        }

        private double GetAngle(List<Vector<double>> positions, int i, int j, int k)
        {
            var r21 = positions[i] - positions[j];
            var r23 = positions[k] - positions[j];

            var e21 = r21.Normalize(p: 2);
            var e23 = r23.Normalize(p: 2);

            var cosValue = e21.DotProduct(e23);
            var angleRad = Math.Acos(cosValue);
            var angleDegree = angleRad / Math.PI * 180.0;

            return angleDegree;
        }
    }
}
