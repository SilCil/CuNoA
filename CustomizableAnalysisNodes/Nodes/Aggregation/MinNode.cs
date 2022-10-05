using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("集計/列/最小")]
    public class MinNode : ICalculationNode, IOptionNode
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
            var min = data.GetColumn(Index).ToDoubleArray().Min();
            return Table.CreateFromSingleElement(new Value(min));
        }
    }
}
