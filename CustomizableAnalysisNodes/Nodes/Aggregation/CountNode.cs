using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/列/データ数")]
    public class CountNode : ICalculationNode, IOptionNode
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
            var count = data.GetColumn(Index).Count;
            return Table.CreateFromSingleElement(new Value(count));
        }
    }
}
