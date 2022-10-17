using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/最後の数行")]
    public class ExtractRowBackUntil : IOptionNode, ICalculationNode
    {
        public int Count { get; set; } = 1;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("取り出す行の数", new Value(Count));
        }

        public void SetOptions(params Value[] options)
        {
            Count = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            var rows = new List<IReadOnlyList<Value>>();
            
            for(int i = data.RowCount - 1; i >= 0; --i)
            {
                if (rows.Count >= Count)
                {
                    break;
                }

                rows.Add(data.GetRow(i));
            }

            rows.Reverse();
            return Table.CreateFromRows(rows);
        }
    }
}
