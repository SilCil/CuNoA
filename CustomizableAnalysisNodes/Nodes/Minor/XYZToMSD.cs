using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using CustomizableAnalysisLibrary;
using System;

namespace Kato.EvAX
{
    [Node("専門/EvAX/<ux2>, <uy2>, <uz2>, <u2>を計算")]
    public class XYZToMSD : ICalculationNode
    {
        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            var labels = finalLabels.Distinct().ToArray();
            var displacementsFromInitial = CalcDisplacements(finalLabels, initialRealPositions, finalRealPositions);
            var averageDisplacements = CalcAverageDisplacements(displacementsFromInitial);

            var rows = new List<Value[]>();
            foreach(var label in finalLabels.Distinct())
            {
                var index = finalLabels.IndexOf(label);
                var initialPosition = initialAllPositions[index];
                var averageOffset = averageDisplacements[label];
                var averagePosition = initialPosition + averageOffset;

                var msdX = displacementsFromInitial[label].Average(p => Math.Pow(p[0] - averageOffset[0], 2.0));
                var msdY = displacementsFromInitial[label].Average(p => Math.Pow(p[1] - averageOffset[1], 2.0));
                var msdZ = displacementsFromInitial[label].Average(p => Math.Pow(p[2] - averageOffset[2], 2.0));
                var msd = msdX + msdY + msdZ;

                rows.Add(new Value[]
                {
                    new Value(label),
                    new Value(msdX),
                    new Value(msdY),
                    new Value(msdZ),
                    new Value(msd),
                });
            }

            return Table.CreateFromRows(rows);
        }

        public static Dictionary<string, List<Vector<double>>> CalcDisplacements(List<string> labels, IReadOnlyList<Vector<double>> initialPositions, IReadOnlyList<Vector<double>> finalPositions)
        {
            var displacementsFromInitial = new Dictionary<string, List<Vector<double>>>();

            for (int i = 0; i < finalPositions.Count; ++i)
            {
                var label = labels[i];
                if (displacementsFromInitial.ContainsKey(label) == false)
                {
                    displacementsFromInitial.Add(label, new List<Vector<double>>());
                }
                displacementsFromInitial[label].Add(finalPositions[i] - initialPositions[i]);
            }

            return displacementsFromInitial;
        }

        public static Dictionary<string, Vector<double>> CalcAverageDisplacements(Dictionary<string, List<Vector<double>>> displacements)
        {
            var averageVectors = new Dictionary<string, Vector<double>>();

            foreach((var label, var vectors) in displacements)
            {
                var sumVector = vectors.Aggregate(Vector<double>.Build.Dense(3, 0), (sum, v) => sum + v);
                averageVectors[label] = sumVector / vectors.Count;
            }

            return averageVectors;
        }
    }
}
