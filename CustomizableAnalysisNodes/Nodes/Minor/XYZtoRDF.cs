using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using CustomizableAnalysisLibrary;

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
        public bool OutputBondLabel { get; set; } = true;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("吸収原子", new Value(Absorber));
            yield return ("最小距離", new Value(MinDistance));
            yield return ("最大距離", new Value(MaxDistance));
            yield return ("bin幅", new Value(DeltaR));
            yield return ("Group閾値", new Value(DistanceEpsilon));
            yield return ("ラベルを出力", new Value(OutputBondLabel));
        }

        public void SetOptions(params Value[] options)
        {
            Absorber = options[0].ToString();
            MinDistance = options[1].ToDouble();
            MaxDistance = options[2].ToDouble();
            DeltaR = options[3].ToDouble();
            DistanceEpsilon = options[4].ToDouble();
            OutputBondLabel = options[5].ToBool();
        }

        public Table Run(Table data)
        {
            XYZReader.GetAtomicConfigurations(data, out var initialLabels, out var initialRealPositions, out var initialAllPositions, out var finalLabels, out var finalRealPositions, out var finalAllPositions);

            int binCount = GetBinIndex(MaxDistance) + 1;
            var rValues = new double[binCount];
            for(int i = 0; i < rValues.Length; ++i)
            {
                rValues[i] = MinDistance + DeltaR * i;
            }

            int absorberCount = 0;
            var bondLabels = new List<string>();
            var initialDistances = new List<double>();
            var rdfValues = new List<double[]>();
            for (int i = 0; i < finalRealPositions.Count; ++i)
            {
                if (finalLabels[i] != Absorber) continue;
                
                ++absorberCount;

                for (int j = 0; j < finalAllPositions.Count; ++j)
                {
                    var initialDistance = Distance.Euclidean(initialAllPositions[i], initialAllPositions[j]);

                    if (initialDistance < MinDistance) continue;
                    if (initialDistance > MaxDistance) continue;

                    var bondLabel = $"{finalLabels[i]}-{finalLabels[j]}";
                    var labelIndex = GetLabelIndex(bondLabels, initialDistances, initialDistance, bondLabel);

                    if (labelIndex < 0)
                    {
                        labelIndex = initialDistances.Count(x => x <= initialDistance);

                        bondLabels.Insert(labelIndex, bondLabel);
                        rdfValues.Insert(labelIndex, new double[binCount]);
                        initialDistances.Insert(labelIndex, initialDistance);
                    }

                    var distance = Distance.Euclidean(finalAllPositions[i], finalAllPositions[j]);
                    if (distance < MinDistance) continue;
                    if (distance > MaxDistance) continue;

                    var binIndex = GetBinIndex(distance);
                    rdfValues[labelIndex][binIndex] += 1.0 / DeltaR;
                }
            }

            for(int i = 0; i < rdfValues.Count; ++i)
            {
                for(int j = 0; j < rdfValues[i].Length; ++j)
                {
                    rdfValues[i][j] /= absorberCount;
                }
            }

            var columns = new List<IEnumerable<Value>>();

            if (OutputBondLabel)
            {
                columns.Add(rValues.ToValueArray().Prepend(new Value("#r")));
            }
            else
            {
                columns.Add(rValues.ToValueArray());
            }

            for(int i = 0; i < bondLabels.Count; ++i)
            {
                IEnumerable<Value> column = rdfValues[i].ToValueArray();

                if (OutputBondLabel)
                {
                    column = column.Prepend(new Value(bondLabels[i]));
                }

                columns.Add(column);
            }

            return Table.CreateFromColumns(columns);
        }

        private int GetLabelIndex(List<string> labels, List<double> initialDistances, double initialDistance, string label)
        {
            for (int i = 0; i < labels.Count; ++i)
            {
                if (labels[i] != label) continue;
                if (Math.Abs(initialDistances[i] - initialDistance) > DistanceEpsilon) continue;
                return i;
            }
            return -1;
        }

        private int GetBinIndex(double r)
        {
            return (int)((r - MinDistance) / DeltaR + 0.5);
        }
    }
}
