using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/列/指定した列番号まで取り出し")]
    public class ExtractColumnUntil : IOptionNode, ICalculationNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToIntValue().IntValue;
        }

        public Table Run(Table data)
        {
            var columns = new List<IReadOnlyList<Value>>();
            
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                if (i <= Index)
                {
                    columns.Add(data.GetColumn(i));
                }
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
