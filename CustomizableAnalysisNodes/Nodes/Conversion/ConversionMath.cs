using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/数値/数式")]
    public class ConversionMath : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;

public static class Code
{
    public static double Evaluate(double x) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public string Code { get; set; } = "x * x";

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("関数", new Value(Code));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToIntValue().IntValue;
            Code = options[1].ToStringValue().StringValue;
        }

        public Table Run(Table data)
        {
            var source = CodeTemplate.Replace("$Code", Code);
            var assembly = Utility.LoadFromSource(key: $"{nameof(CustomizableAnalysisLibrary.Nodes)}:{nameof(ConversionMath)}:{Code}", source);
            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            var evaluate = (Func<double, double>)Delegate.CreateDelegate(typeof(Func<double, double>), evaluateInfo);

            var rows = new List<IReadOnlyList<Value>>();
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                var x = row[IndexX].ToDoubleValue().DoubleValue;
                row[IndexX] = new Value(evaluate(x));
                rows.Add(row);
            }
            return Table.CreateFromRows(rows);
        }
    }
}
