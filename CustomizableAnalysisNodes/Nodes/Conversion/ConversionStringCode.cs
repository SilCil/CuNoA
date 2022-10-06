using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/文字列/プログラム")]
    public class ConversionStringCode : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;

public static class Code
{
    public static string Evaluate(string x) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public string Code { get; set; } = "x.Substring(0, 1)";

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("コード", new Value(Code));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToInt();
            Code = options[1].ToString();
        }

        public Table Run(Table data)
        {
            var source = CodeTemplate.Replace("$Code", Code);
            var assembly = Utility.LoadFromSource(key: $"{nameof(CustomizableAnalysisLibrary.Nodes)}:{nameof(ConversionStringCode)}:{Code}", source);
            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            var evaluate = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), evaluateInfo);

            var rows = new List<IReadOnlyList<Value>>();
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                var x = row[IndexX].ToString();
                row[IndexX] = new Value(evaluate(x));
                rows.Add(row);
            }
            return Table.CreateFromRows(rows);
        }
    }
}
