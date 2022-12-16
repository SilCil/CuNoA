using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/数値/数式（x, y）")]
    public class ConversionMathXY : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;
using CustomizableAnalysisLibrary;

public static class Code
{
    public static double Evaluate(double x, double y) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 1;
        public bool ReplaceColumnY { get; set; } = true;
        public string Code { get; set; } = "x + y";

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("yの列番号", new Value(IndexY));
            yield return ("yの列を上書き", new Value(ReplaceColumnY));
            yield return ("関数", new Value(Code));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToInt();
            IndexY = options[1].ToInt();
            ReplaceColumnY = options[2].ToBool();
            Code = options[3].ToString();
        }

        public Table Run(Table data)
        {
            var source = CodeTemplate.Replace("$Code", Code);
            var assembly = Utility.LoadFromSource(key: $"{nameof(Nodes)}:{nameof(ConversionMathXY)}:{Code}", source);
            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            var evaluate = (Func<double, double, double>)Delegate.CreateDelegate(typeof(Func<double, double, double>), evaluateInfo);

            var rows = new List<IReadOnlyList<Value>>();
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                var x = row[IndexX].ToDouble();
                var y = row[IndexY].ToDouble();

                var result = new Value(evaluate(x, y));

                if (ReplaceColumnY)
                {
                    row[IndexY] = result;
                    rows.Add(row);
                }
                else
                {
                    rows.Add(row.Append(result).ToArray());
                }
            }
            return Table.CreateFromRows(rows);
        }
    }
}
