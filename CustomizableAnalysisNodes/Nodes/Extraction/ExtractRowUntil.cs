using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/最初の数行")]
    public class ExtractRowUntil : IOptionNode, ICalculationNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("取り出す行の数", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            var rows = new List<IReadOnlyList<Value>>();
            
            for(int i = 0; i < data.RowCount; ++i)
            {
                if (i < Index)
                {
                    rows.Add(data.GetRow(i));
                }
            }

            return Table.CreateFromRows(rows);
        }
    }
}
