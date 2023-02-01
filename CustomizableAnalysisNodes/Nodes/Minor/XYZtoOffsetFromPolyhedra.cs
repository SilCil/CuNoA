using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using CustomizableAnalysisLibrary;
using MathNet.Numerics.LinearAlgebra;

namespace Kato.EvAX
{
    [Node("専門/EvAX/オフセンター変位<x>, <y>, <z>, <r>, <x2>, <y2>, <z2>, <r2>, <ux>, <uy>, <uz>, <u>, <ux2>, <uy2>, <uz2>, <u2>")]
    public class XYZtoOffsetFromPolyhedra : ICalculationNode, IOptionNode
    {
        private const double DistanceCutoff = 0.0001;

        public string CenterLabel { get; set; } = "Ti";
        public string CoordinateLabel { get; set; } = "O";
        public int CoordinationNumber { get; set; } = 6;
        public double MaxDistance { get; set; } = 6.0;
        public bool GroupBySite { get; set; } = false;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("中心原子", new Value(CenterLabel));
            yield return ("配位原子", new Value(CoordinateLabel));
            yield return ("配位数", new Value(CoordinationNumber));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("Siteを区別する", new Value(GroupBySite));
        }

        public void SetOptions(params Value[] options)
        {
            CenterLabel = options[0].ToString();
            CoordinateLabel = options[1].ToString();
            CoordinationNumber = options[2].ToInt();
            MaxDistance = options[3].ToDouble();
            GroupBySite = options[4].ToBool();
        }

        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var _, out var __, out var ___, out var labels, out var realPositions, out var allPositions);

            if (GroupBySite == false)
            {
                labels = labels.Select(x => RemoveNumber(x)).ToList();
            }

            var offsets = new List<Vector<double>>();
            foreach ((var centerLabel, var centerPosition) in labels.Zip(realPositions, (l, p) => (l, p)))
            {
                if (centerLabel != CenterLabel) continue;

                var distances = Enumerable.Repeat(double.MaxValue, CoordinationNumber).ToArray();
                var positions = new Vector<double>[CoordinationNumber];
                foreach((var label, var pos) in labels.Zip(allPositions, (l, p) => (l, p)))
                {
                    if (label != CoordinateLabel) continue;

                    var distance = Distance.Euclidean(centerPosition, pos);
                    if (distance > MaxDistance) continue;
                    if (distance < DistanceCutoff) continue;

                    InsertDistance(distance, pos, ref distances, ref positions);
                }

                var oncenterPosition = positions.Aggregate(Vector<double>.Build.Dense(3, 0.0), (sum, p) => sum + p) / positions.Length;
                var offset = centerPosition - oncenterPosition;
                offsets.Add(offset);
            }

            var averageOffset = offsets.Aggregate(Vector<double>.Build.Dense(3, 0.0), (sum, p) => sum + p) / offsets.Count;
            var r = offsets.Average(v => v.L2Norm());
            var x2 = offsets.Average(v => v[0] * v[0]);
            var y2 = offsets.Average(v => v[1] * v[1]);
            var z2 = offsets.Average(v => v[2] * v[2]);
            var r2 = offsets.Average(v => v.DotProduct(v));
            var uVectors = offsets.Select(v => v - averageOffset).ToArray();
            var ux = uVectors.Average(v => v[0]);
            var uy = uVectors.Average(v => v[1]);
            var uz = uVectors.Average(v => v[2]);
            var u = uVectors.Average(v => v.L2Norm());
            var ux2 = uVectors.Average(v => v[0] * v[0]);
            var uy2 = uVectors.Average(v => v[1] * v[1]);
            var uz2 = uVectors.Average(v => v[2] * v[2]);
            var u2 = uVectors.Average(v => v.DotProduct(v));

            var values = new List<Value>();
            values.Add(new Value(averageOffset[0]));
            values.Add(new Value(averageOffset[1]));
            values.Add(new Value(averageOffset[2]));
            values.Add(new Value(r));
            values.Add(new Value(x2));
            values.Add(new Value(y2));
            values.Add(new Value(z2));
            values.Add(new Value(r2));
            values.Add(new Value(uz));
            values.Add(new Value(uy));
            values.Add(new Value(uz));
            values.Add(new Value(u));
            values.Add(new Value(ux2));
            values.Add(new Value(uy2));
            values.Add(new Value(uz2));
            values.Add(new Value(u2));

            return Table.CreateFromRows(values);
        }

        private void InsertDistance(double distance, Vector<double> position, ref double[] distances, ref Vector<double>[] positions)
        {
            var insertIndex = distances.Count(x => x < distance);
            if (insertIndex >= distances.Length) return;

            for(int i = distances.Length - 2; i >= insertIndex; --i)
            {
                distances[i + 1] = distances[i];
                positions[i + 1] = positions[i];
            }

            distances[insertIndex] = distance;
            positions[insertIndex] = position;
        }

        private string RemoveNumber(string str)
        {
            return new string(str.Where(x => char.IsDigit(x) == false).ToArray());
        }
    }
}
