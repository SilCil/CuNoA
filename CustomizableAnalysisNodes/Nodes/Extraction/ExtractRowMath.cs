using System;
using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/数値/条件を数式で入力する")]
    public class ExtractRowMath : ICalculationNode, IOptionNode
    {
        private const string CodeTemplate = @"
using System;

public static class Code
{
    public static bool Evaluate(double x) => $Code;
}
";

        public int IndexX { get; set; } = 0;
        public string Code { get; set; } = "0 <= x && x <= 10";

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("xの列番号", new Value(IndexX));
            yield return ("取り出し条件", new Value(Code));
        }

        public void SetOptions(params Value[] options)
        {
            IndexX = options[0].ToInt();
            Code = options[1].ToString();
        }

        public Table Run(Table data)
        {
            var source = CodeTemplate.Replace("$Code", Code);
            var assembly = Utility.LoadFromSource(key: $"{nameof(Nodes)}:{nameof(ExtractRowMath)}:{Code}", source);
            var codeType = assembly.GetType("Code");
            var evaluateInfo = codeType.GetMethod("Evaluate");
            var evaluate = (Func<double, bool>)Delegate.CreateDelegate(typeof(Func<double, bool>), evaluateInfo);

            var rows = new List<IReadOnlyList<Value>>();
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                var x = row[IndexX].ToDouble();
                if (evaluate.Invoke(x))
                {
                    rows.Add(row);
                }
            }

            return Table.CreateFromRows(rows);
        }
    }
}
