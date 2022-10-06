using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/番号を指定")]
    public class ExtractRow : ICalculationNode, IOptionNode
    {
        public int[] Indices { get; set; } = new int[] { 0, 1 };

        public IEnumerable<(string label, Value)> GetOptions()
        {
            var count = new Value(Indices is null ? 0 : Indices.Length);
            yield return ("取り出す行の数", count);

            for (int i = 0; i < count.IntValue; ++i)
            {
                yield return ($"行番号{i}", new Value(Indices[i]));
            }
        }

        public void SetOptions(params Value[] options)
        {
            if (options is null || options.Length == 0)
            {
                Indices = new int[0];
            }
            else
            {
                Indices = new int[GetValue(options[0])];
            }

            for (int i = 0; i < Indices.Length; ++i)
            {
                Indices[i] = (i + 1 < options.Length) ? GetValue(options[i + 1]) : GetValue(options.Last());
            }
        }

        private int GetValue(Value value)
        {
            var v = value.ToInt();
            return (v < 0) ? 0 : v;
        }

        public Table Run(Table data)
        {
            var rows = new List<IReadOnlyList<Value>>();
            
            for(int i = 0; i < data.RowCount; ++i)
            {
                if (Indices.Contains(i))
                {
                    rows.Add(data.GetRow(i));
                }
            }
            
            return Table.CreateFromRows(rows);
        }
    }
}
