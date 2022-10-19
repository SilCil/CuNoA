using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomizableAnalysisLibrary;
using MathNet.Numerics.LinearAlgebra;

namespace Kato.EvAX
{
    [Node("専門/EvAX/初期位置と終了位置を読み込み")]
    public class XYZReader : IInputNode, IOptionNode
    {
        public InputType InputType => InputType.Folder;

        public string InitialFile { get; set; } = "initial.xyz";
        public string FinalFile { get; set; } = "final.xyz";

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("初期位置ファイル", new Value(InitialFile));
            yield return ("終了位置ファイル", new Value(FinalFile));
        }

        public void SetOptions(params Value[] options)
        {
            InitialFile = options[0].ToString();
            FinalFile = options[1].ToString();
        }

        public Table Load(string path)
        {
            var dir = Path.ChangeExtension(Path.GetFullPath(path), "");

            ReadXYZFile(dir, InitialFile, out var initialRealAtoms, out var initialAllAtoms);
            ReadXYZFile(dir, FinalFile, out var finalRealAtoms, out var finalAllAtoms);

            var rows = new List<IEnumerable<Value>>();

            foreach((var initial, var final) in initialRealAtoms.Zip(finalRealAtoms))
            {
                rows.Add(CreateRow(initial, final));
            }
            rows.Add(CreateEmptyRow());

            foreach ((var initial, var final) in initialAllAtoms.Zip(finalAllAtoms).Skip(initialRealAtoms.Count))
            {
                rows.Add(CreateRow(initial, final));
            }

            return Table.CreateFromRows(rows);
        }

        private static void ReadXYZFile(string dir, string file, out List<Value[]> realAtoms, out List<Value[]> allAtoms)
        {
            var path = Directory.GetFiles(dir, "*", SearchOption.AllDirectories).FirstOrDefault(x => Path.GetFileName(x) == file);
            if (path is null) throw new Exception($"{file} not found.");

            var lines = File.ReadAllLines(path);
            var allCount = int.Parse(lines[0]);
            var realCount = int.Parse(lines[1].Split()[1]);

            realAtoms = new List<Value[]>(capacity: realCount);
            allAtoms = new List<Value[]>(capacity: allCount);
            for(int i = 0; i < allCount; ++i)
            {
                var words = lines[i + 2].Split();
                var values = words.Take(4).ToValueArray(); // Label, x, y, z.
                
                if (i < realCount) realAtoms.Add(values);
                allAtoms.Add(values);
            }
        }

        private static IEnumerable<Value> CreateRow(Value[] initial, Value[] final)
        {
            yield return initial[0];
            yield return final[0];
            yield return initial[1];
            yield return initial[2];
            yield return initial[3];
            yield return final[1];
            yield return final[2];
            yield return final[3];
        }

        private static IEnumerable<Value> CreateEmptyRow() => Enumerable.Repeat(new Value(string.Empty), 8);

        public static void GetAtomicConfigurations(Table data, out List<string> initialLabels, out List<Vector<double>> initialRealPositions, out List<Vector<double>> initialAllPositions, out List<string> finalLabels, out List<Vector<double>> finalRealPositions, out List<Vector<double>> finalAllPositions)
        {
            initialLabels = new List<string>();
            initialRealPositions = new List<Vector<double>>();
            initialAllPositions = new List<Vector<double>>();
            finalLabels = new List<string>();
            finalRealPositions = new List<Vector<double>>();
            finalAllPositions = new List<Vector<double>>();
            bool isRealAtom = true;
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);

                var initialLabel = row[0].ToString();
                if (string.IsNullOrWhiteSpace(initialLabel))
                {
                    isRealAtom = false;
                    continue;
                }

                var finalLabel = row[1].ToString();
                var initialPos = Vector<double>.Build.Dense(3);
                var finalPos = Vector<double>.Build.DenseOfVector(initialPos);

                initialPos[0] = row[2].ToDouble();
                initialPos[1] = row[3].ToDouble();
                initialPos[2] = row[4].ToDouble();

                finalPos[0] = row[5].ToDouble();
                finalPos[1] = row[6].ToDouble();
                finalPos[2] = row[7].ToDouble();

                initialLabels.Add(initialLabel);
                initialAllPositions.Add(initialPos);

                finalLabels.Add(finalLabel);
                finalAllPositions.Add(finalPos);

                if (isRealAtom == false) continue;
                initialRealPositions.Add(initialPos);
                finalRealPositions.Add(finalPos);
            }
        }
    }
}
