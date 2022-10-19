using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/文字列/数字のみを取り出す")]
    public class PickupDigitString : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            var rows = new List<Value[]>();
            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i).ToArray();
                row[Index] = PickupDigit(row[Index]);
                rows.Add(row);
            }
            return Table.CreateFromRows(rows);
        }

        private static Value PickupDigit(in Value value)
        {
            var str = value.ToString();
            var picked = new string(str.Where(c => char.IsDigit(c)).ToArray());
            return new Value(picked);
        }
    }
}
