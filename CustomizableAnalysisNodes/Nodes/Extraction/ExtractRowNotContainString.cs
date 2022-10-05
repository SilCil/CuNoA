using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/文字列/特定の文字を含まない")]
    public class ExtractRowNotContainString : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;
        public string Search { get; set; } = default;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
            yield return ("検索文字", new Value(Search));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToIntValue().IntValue;
            Search = options[1].ToStringValue().StringValue;
        }

        public Table Run(Table data)
        {
            var rows = new List<IEnumerable<Value>>();

            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                var word = row[Index].ToStringValue().StringValue;

                if (word.Contains(Search) == false)
                {
                    rows.Add(row);
                }
            }

            return Table.CreateFromRows(rows);
        }
    }
}
