using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/文字列/置換")]
    public class ReplaceString : IOptionNode, ICalculationNode
    {
        public int Index { get; set; } = 0;
        public string OldString { get; set; } = default;
        public string NewString { get; set; } = default;
        public bool UseRegularExpressions { get; set; } = false;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
            yield return ("検索", new Value(OldString));
            yield return ("置換", new Value(NewString));
            yield return ("正規表現を使用する", new Value(UseRegularExpressions));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
            OldString = options[1].ToString();
            NewString = options[2].ToString();
            UseRegularExpressions = options[3].ToBool();
        }

        public Table Run(Table data)
        {
            var rows = new List<Value[]>();
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                row[Index] = Replace(row[Index]);
                rows.Add(row);
            }
            return Table.CreateFromRows(rows);
        }

        private Value Replace(in Value value)
        {
            var str = value.ToString();
            if (UseRegularExpressions)
            {
                return new Value(Regex.Replace(str, OldString, NewString));
            }
            else
            {
                return new Value(str.Replace(OldString, NewString));
            }
        }
    }
}
