using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/列/最後の数列")]
    public class ExtractColumnBackUntil : IOptionNode, ICalculationNode
    {
        public int Count { get; set; } = 1;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("取り出す列の数", new Value(Count));
        }

        public void SetOptions(params Value[] options)
        {
            Count = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            var columns = new List<IReadOnlyList<Value>>();
            
            for(int i = data.ColumnCount - 1; i >= 0; --i)
            {
                if (columns.Count >= Count)
                {
                    break;
                }

                columns.Add(data.GetColumn(i));
            }

            columns.Reverse();
            return Table.CreateFromColumns(columns);
        }
    }
}
