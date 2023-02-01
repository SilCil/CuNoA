using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using CustomizableAnalysisLibrary;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace Kato.EvAX
{
    [Node("専門/EvAX/基準軸からのオフセンター変位<r>, <r2>, <r_perpendicular>, <r_perpendicular2>, <theta>")]
    public class XYZtoProjectedOffsetFromPolyhedra : ICalculationNode, IOptionNode
    {
        private const double DistanceCutoff = 0.0001;

        public string CenterLabel { get; set; } = "Ti";
        public string CoordinateLabel { get; set; } = "O";
        public int CoordinationNumber { get; set; } = 6;
        public double MaxDistance { get; set; } = 6.0;
        public double AxisX { get; set; } = 0.0;
        public double AxisY { get; set; } = 0.0;
        public double AxisZ { get; set; } = 1.0;
        public bool GroupBySite { get; set; } = false;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("中心原子", new Value(CenterLabel));
            yield return ("配位原子", new Value(CoordinateLabel));
            yield return ("配位数", new Value(CoordinationNumber));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("基準軸x", new Value(AxisX));
            yield return ("基準軸y", new Value(AxisY));
            yield return ("基準軸z", new Value(AxisZ));
            yield return ("Siteを区別する", new Value(GroupBySite));
        }

        public void SetOptions(params Value[] options)
        {
            CenterLabel = options[0].ToString();
            CoordinateLabel = options[1].ToString();
            CoordinationNumber = options[2].ToInt();
            MaxDistance = options[3].ToDouble();
            AxisX = options[4].ToDouble();
            AxisY = options[5].ToDouble();
            AxisZ = options[6].ToDouble();
            GroupBySite = options[7].ToBool();
        }

        public Table Run(Table data)
        {
            var axisRaw = Vector<double>.Build.Dense(new double[] { AxisX, AxisY, AxisZ });
            var axis = axisRaw.Normalize(2.0);

            XYZReader.GetAtomicConfigurations(data, out var _, out var __, out var ___, out var labels, out var realPositions, out var allPositions);

            if (GroupBySite == false)
            {
                labels = labels.Select(x => RemoveNumber(x)).ToList();
            }

            var rValues = new List<double>();
            var r2Values = new List<double>();
            var rPerpValues = new List<double>();
            var rPerp2Values = new List<double>();
            var thetaValues = new List<double>();
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

                var parallel = axis.DotProduct(offset);
                var theta = Math.Acos(parallel / offset.L2Norm()) * 180.0 / Math.PI;
                var perpendicular = (offset - parallel * axis).L2Norm();
                rValues.Add(parallel);
                r2Values.Add(parallel * parallel);
                rPerpValues.Add(perpendicular);
                rPerp2Values.Add(perpendicular * perpendicular);
                thetaValues.Add(theta);
            }

            var values = new List<double>();
            values.Add(rValues.Average());
            values.Add(r2Values.Average());
            values.Add(rPerpValues.Average());
            values.Add(rPerp2Values.Average());
            values.Add(thetaValues.Average());

            return Table.CreateFromRows(values.ToValueArray());
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
